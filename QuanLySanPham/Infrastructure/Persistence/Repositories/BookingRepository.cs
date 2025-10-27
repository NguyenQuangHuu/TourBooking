using Npgsql;
using NpgsqlTypes;
using QuanLySanPham.Domain.Aggregates.Bookings;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Infrastructure.Exceptions;

namespace QuanLySanPham.Infrastructure.Persistence.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public BookingRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Booking> CreateBookingAsync(Booking booking, CancellationToken ct)
    {
        string sql =
            "insert into bookings(total_amount, booking_status,tour_instance_id,user_id) values(@TotalSlots,@BookingStatus,@TourInstanceId,@UserId) returning id ";

        await using NpgsqlCommand cmd =
            new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        cmd.Parameters.Add(new NpgsqlParameter("@TotalSlots", booking.Total.Value));
        cmd.Parameters.Add(new NpgsqlParameter("@BookingStatus", booking.BookingStatus.Value));
        cmd.Parameters.Add(new NpgsqlParameter("@TourInstanceId", booking.TourInstanceId.Value));
        cmd.Parameters.Add(new NpgsqlParameter("@UserId", booking.UserId.Value));
        object? result = await cmd.ExecuteScalarAsync(ct);
        if (result is null)
        {
            throw new InfrastructureException("Cannot insert");
        }
        BookingId bookingId  = BookingId.From((Guid)result);
        booking.Id = bookingId;
        return booking;
    }

    public async Task UpdateBookingAsync(Booking booking, CancellationToken ct)
    {
        string sql =
            "update bookings set total_amount = @TotalSlots, booking_status = @BookingStatus,tour_instance_id = @TourInstanceId,user_id = @UserId where id = @BookingId ";

        await using NpgsqlCommand cmd =
            new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        cmd.Parameters.Add(new NpgsqlParameter("@TotalSlots", booking.Total.Value));
        cmd.Parameters.Add(new NpgsqlParameter("@BookingStatus", booking.BookingStatus.Value));
        cmd.Parameters.Add(new NpgsqlParameter("@TourInstanceId", booking.TourInstanceId.Value));
        cmd.Parameters.Add(new NpgsqlParameter("@UserId", booking.UserId.Value));
        cmd.Parameters.Add(new NpgsqlParameter("@BookingId", booking.Id.Value));
        int result = await cmd.ExecuteNonQueryAsync(ct);
        if (result<0)
        {
            throw new InfrastructureException("Cannot insert");
        }
    }

    public async Task<Booking?> GetBookingByIdAsync(BookingId bookingId, CancellationToken ct)
    {
        string sql = "select id,user_id,tour_instance_id,total_amount,booking_status from bookings where id = @Id";
        await using NpgsqlCommand cmd = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        cmd.Parameters.Add(new NpgsqlParameter("@Id", bookingId.Value));
        await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return new Booking
            {
                Id = BookingId.From(reader.GetGuid(0)),
                UserId = UserId.From(reader.GetGuid(1)),
                TourInstanceId = TourInstanceId.From(reader.GetGuid(2)),
                Total = new Quantity(reader.GetInt32(3)),
                BookingStatus = BookingStatus.From(reader.GetString(4))
            };
        }

        return null;
    }

    public async Task<int> AddPassengersByBookingIdAsync(BookingId id, List<Passenger> passengers,UserId userId, CancellationToken ct)
    {
        string sql =
            "insert into passengers(full_name, dob, identity_no, passport_no, gender, phone_number, email, address, booking_id, user_id) " +
            "values(@FullName,@Dob,@IdentityNo,@PassportNo,@Gender,@PhoneNumber,@Email,@Address,@BookingId,@UserId)";
        var result = 0;
        foreach (var passenger in passengers)
        {
            await using NpgsqlCommand cmd = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
            
            // Thêm parameters
            cmd.Parameters.AddWithValue("@FullName", NpgsqlDbType.Text, passenger.PassengerInfo.FullName);
            cmd.Parameters.AddWithValue("@Dob",NpgsqlDbType.Date, passenger.PassengerInfo.DateOfBirth);
            cmd.Parameters.AddWithValue("@IdentityNo", NpgsqlDbType.Text, passenger.PassengerInfo.IdentityCardNo);
            cmd.Parameters.AddWithValue("@PassportNo",   NpgsqlDbType.Text, passenger.PassengerInfo.PassportNo);
            cmd.Parameters.AddWithValue("@Gender",  NpgsqlDbType.Text, passenger.PassengerInfo.Gender.ToString());
            cmd.Parameters.AddWithValue("@PhoneNumber",  NpgsqlDbType.Text, passenger.PassengerContact.Phone);
            cmd.Parameters.AddWithValue("@Email", NpgsqlDbType.Text, passenger.PassengerContact.Email);
            cmd.Parameters.AddWithValue("@Address",  NpgsqlDbType.Text, passenger.PassengerContact.Address);
            cmd.Parameters.AddWithValue("@BookingId",  NpgsqlDbType.Uuid, id.Value);
            cmd.Parameters.AddWithValue("@UserId",  NpgsqlDbType.Uuid, userId.Value);
            result += await cmd.ExecuteNonQueryAsync(ct);
        }
        return result;
    }
}