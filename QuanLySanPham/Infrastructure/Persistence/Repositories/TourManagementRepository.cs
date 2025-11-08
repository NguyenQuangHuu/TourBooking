using Npgsql;
using QuanLySanPham.Domain.Aggregates.Tours;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Infrastructure.Commons;
using QuanLySanPham.Infrastructure.Interfaces;

namespace QuanLySanPham.Infrastructure.Persistence.Repositories;

public class TourManagementRepository : ITourManagementRepository
{
    private readonly IUnitOfWork _uow;

    public TourManagementRepository(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<TourMasterId?> CreateTourMaster(TourMaster tourMaster, CancellationToken token = default)
    {
        var sql =
            @"insert into tour_master(name,duration_estimate,operating_cost) values(@TourName,@Duration,@Cost) returning id";
        await using var command = new NpgsqlCommand(sql, _uow.Connection, _uow.Transaction);
        command.Parameters.Add(new NpgsqlParameter("@TourName", tourMaster.Name));
        command.Parameters.Add(new NpgsqlParameter("@Duration", tourMaster.DurationEstimate.Value));
        command.Parameters.Add(new NpgsqlParameter("@Cost", tourMaster.OperatingCost.Amount));
        var result = await command.ExecuteScalarAsync(token);
        //Pattern Matching
        if (result is not null) return TourMasterId.From((Guid)result);

        return null;
    }

    public Task<TourMaster?> GetTourMasterByTourName(string tourName, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<TourMaster>> GetAllTourMasters(CancellationToken cancellationToken)
    {
        var sql = @"select * from tour_master";
        await using var cmd = new NpgsqlCommand(sql, _uow.Connection);
        await using var read = await cmd.ExecuteReaderAsync(cancellationToken);
        List<TourMaster> tourMasters = new();
        while (await read.ReadAsync(cancellationToken))
        {
            var tour = new TourMaster
            {
                Id = TourMasterId.From(read.GetGuid(0)),
                Name = read.GetString(1),
                DurationEstimate = new TimeSchedule(read.GetInt32(2)),
                OperatingCost = new Money(read.GetDouble(3))
            };
            tourMasters.Add(tour);
        }

        return tourMasters;
    }

    public async Task<TourMaster?> GetTourMasterById(TourMasterId id, CancellationToken token = default)
    {
        var sql = @"select * from tour_master where id = @TourMasterId";
        await using var cmd = new NpgsqlCommand(sql, _uow.Connection);
        cmd.Parameters.Add(new NpgsqlParameter("@TourMasterId", id.Value));
        await using var reader = await cmd.ExecuteReaderAsync(token);
        if (await reader.ReadAsync(token))
            return new TourMaster
            {
                Id = TourMasterId.From(reader.GetGuid(0)),
                Name = reader.GetString(1),
                DurationEstimate = new TimeSchedule(reader.GetInt32(2)),
                OperatingCost = new Money(reader.GetDouble(3))
            };
        return null;
    }

    public async Task<TourInstanceId?> CreateTourInstance(TourInstance tourInstance, CancellationToken token = default)
    {
        var sql =
            @"insert into tour_instance(price_per_pax,start_date,end_date,booked_slots,opened_slots,available_slots,tour_master_id)" +
            "values (@PricePerPax,@StartDate,@EndDate,@BookedSlots,@OpenedSlots,@AvailableSlots,@TourMasterId) returning id";
        await using var cmd = new NpgsqlCommand(sql, _uow.Connection);
        cmd.Parameters.Add(new NpgsqlParameter("@PricePerPax", tourInstance.PricePerPax.Amount));
        cmd.Parameters.Add(new NpgsqlParameter("@StartDate", tourInstance.OperationalPeriod.Start));
        cmd.Parameters.Add(new NpgsqlParameter("@EndDate", tourInstance.OperationalPeriod.End));
        cmd.Parameters.Add(new NpgsqlParameter("@BookedSlots", "0"));
        cmd.Parameters.Add(new NpgsqlParameter("@OpenedSlots", tourInstance.SlotInfo.OpenedSlot));
        cmd.Parameters.Add(new NpgsqlParameter("@AvailableSlots", tourInstance.SlotInfo.OpenedSlot));
        cmd.Parameters.Add(new NpgsqlParameter("@TourMasterId", tourInstance.TourMasterId.Value));
        var result = await cmd.ExecuteScalarAsync(token);
        if (result is TourInstanceId id) return id;

        return null;
    }


    public async Task InsertTourMasterDestinations(TourMasterId tourMasterId, List<TourMasterDestination> listIds,
        CancellationToken token)
    {
        if (listIds.Any())
            foreach (var destination in listIds)
            {
                var destinationSql =
                    @"insert into tour_master_destination(tour_master_id, destination_id, order_number, stay_days) " +
                    "VALUES (@TourMasterId,@DestinationId,@Order,@StayDays) returning id";
                object? destinationObj = new
                {
                    TourMasterId = tourMasterId.Value,
                    DestinationId = destination.DestinationId.Value,
                    Order = destination.Order,
                    StayDays = destination.StayDays
                };
                Func<NpgsqlCommand, Task<object?>> destinationCommand = async cmd =>
                {
                    return await cmd.ExecuteNonQueryAsync(token);
                };
            }

        await Task.CompletedTask;
    }

    public async Task InsertTourMasterPoi(TourMasterId tourMasterId, List<TourMasterPoi> listIds,
        CancellationToken token)
    {
        if (listIds.Any())
            foreach (var poi in listIds)
            {
                var poiSql = @"insert into tour_master_poi(tour_master_id, destination_poi_id, day_order)" +
                             "values(@TourMasterId,@DestinationPoiId,@DayOrder)";
                object? poiObj = new
                {
                    TourMasterId = tourMasterId.Value,
                    DestinationPoiId = poi.PointOfInterestId.Value,
                    DayOrder = poi.Order
                };
                Func<NpgsqlCommand, Task<object?>> poiCommand = async cmd =>
                {
                    return await cmd.ExecuteNonQueryAsync(token);
                };
            }

        await Task.CompletedTask;
    }


    public async Task<IReadOnlyList<TourInstance>> GetTourInstancesByTourMasterIdAsync(TourMasterId tourMasterId,
        CancellationToken token = default)
    {
        var sql = "select * from tour_instance where tour_master_id = @Id";
        await using var cmd = new NpgsqlCommand(sql, _uow.Connection);
        List<TourInstance> list = new();
        await using var reader = await cmd.ExecuteReaderAsync(token);
        while (await reader.ReadAsync(token))
        {
            var instance = new TourInstance
            {
                Id = TourInstanceId.From(reader.GetGuid(0)),
                PricePerPax = new Money(reader.GetDouble(1)),
                OperationalPeriod = new DateRange(DateOnly.FromDateTime(reader.GetDateTime(2)),
                    DateOnly.FromDateTime(reader.GetDateTime(3))),
                SlotInfo = new SlotInfo(reader.GetInt32(5), reader.GetInt32(4))
            };
            list.Add(instance);
        }

        return list;
    }

    public async Task<TourInstance?> GetTourInstanceByIdAsync(TourInstanceId tourInstanceId, CancellationToken ct)
    {
        string sql = "select * from tour_instance where id = @TourInstanceId";
        await using var command = new NpgsqlCommand(sql, _uow.Connection);
        command.Parameters.Add(new NpgsqlParameter("@TourInstanceId", tourInstanceId.Value));
        await using var reader = await command.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return new TourInstance
            {
                Id = TourInstanceId.From(reader.GetGuid(0)),
                PricePerPax = new Money(reader.GetDouble(1)),
                OperationalPeriod  = new DateRange(DateOnly.FromDateTime(reader.GetDateTime(2)),DateOnly.FromDateTime(reader.GetDateTime(3))),
                SlotInfo = new SlotInfo(reader.GetInt32(5), reader.GetInt32(4))
            };
        }

        return null;
    }

    public async Task<int> UpdateTourInstanceAsync(TourInstance tourInstance, CancellationToken token)
    {
        string sql =
            "update tour_instance set price_per_pax= @PricePerPax,booked_slots = @BookedSlot,opened_slots = @OpenedSlot,available_slots = @AvailableSlot where id = @Id";
        await using NpgsqlCommand cmd = new NpgsqlCommand(sql, _uow.Connection, _uow.Transaction);
        cmd.Parameters.Add(new NpgsqlParameter("@Id", tourInstance.Id.Value));
        cmd.Parameters.Add(new NpgsqlParameter("@PricePerPax", tourInstance.PricePerPax.Amount));
        cmd.Parameters.Add(new NpgsqlParameter("@OpenedSlot", tourInstance.SlotInfo.OpenedSlot));
        cmd.Parameters.Add(new NpgsqlParameter("@BookedSlot", tourInstance.SlotInfo.BookedSlot));
        cmd.Parameters.Add(new NpgsqlParameter("@AvailableSlot", tourInstance.SlotInfo.AvailableSlot));
        var result = await cmd.ExecuteNonQueryAsync(token);
        return result;
    }
}