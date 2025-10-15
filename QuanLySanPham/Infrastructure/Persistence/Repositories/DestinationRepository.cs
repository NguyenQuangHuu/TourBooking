using Npgsql;
using QuanLySanPham.Domain.Aggregates.Destinations;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Infrastructure.Commons;
using QuanLySanPham.Infrastructure.Interfaces;

namespace QuanLySanPham.Infrastructure.Persistence.Repositories;

public class DestinationRepository : IDestinationRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public DestinationRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DestinationId?> CreateDestinationAsync(Destination destination, CancellationToken token)
    {
        var sql = @"INSERT INTO destinations(name,country,is_oversea) values(@Name,@Country,@IsOverSea) returning id";
        await using var command = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        command.Parameters.Add(new NpgsqlParameter("@Name", destination.Name));
        command.Parameters.Add(new NpgsqlParameter("@Country", destination.Country));
        command.Parameters.Add(new NpgsqlParameter("@IsOverSea", destination.IsOverSea));
        var result = await command.ExecuteScalarAsync(token);
        if (result is DestinationId id) return id;

        return null;
    }

    public async Task<Destination?> GetDestinationByNameAsync(string name, CancellationToken token)
    {
        var sql = @"SELECT * FROM destinations WHERE name = @Name";
        await using var command = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        command.Parameters.Add(new NpgsqlParameter("@Name", name));
        await using var reader = await command.ExecuteReaderAsync(token);
        if (await reader.ReadAsync(token))
            return new Destination
            {
                Id = DestinationId.From(reader.GetGuid(0)),
                Name = reader.GetString(1),
                Country = reader.GetString(2),
                IsOverSea = reader.GetBoolean(3)
            };

        return null;
    }

    public async Task<Destination?> GetDestinationByIdAsync(DestinationId id, CancellationToken token)
    {
        var sql = "select d.id ,d.name,d.country,d.is_oversea from destinations d where d.id = @Id";
        await using var command = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        command.Parameters.Add(new NpgsqlParameter(@"id", id));
        await using var reader = await command.ExecuteReaderAsync(token);
        if (await reader.ReadAsync(token))
        {
            var destination = new Destination
            {
                Id = DestinationId.From(reader.GetGuid(0)),
                Name = reader.GetString(1),
                Country = reader.GetString(2),
                IsOverSea = reader.GetBoolean(3)
            };
            return destination;
        }

        return null;
    }

    public async Task<Destination?> GetDestinationWithPoiByIdAsync(DestinationId id, CancellationToken token)
    {
        var sql =
            "select d.id ,d.name,d.country,d.is_oversea,p.id ,p.name,p.duration,p.poi_type,p.description,p.destination_id from destinations d inner join destination_poi p on d.id = p.destination_id where d.id = @Id";
        await using var command = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        command.Parameters.Add(new NpgsqlParameter("@Id", id));
        await using var reader = await command.ExecuteReaderAsync(token);
        Destination? des = null;
        while (await reader.ReadAsync(token))
        {
            if (des is null)
                des = new Destination
                {
                    Id = DestinationId.From(reader.GetGuid(0)),
                    Name = reader.GetString(1),
                    Country = reader.GetString(2),
                    IsOverSea = reader.GetBoolean(3)
                };

            if (!await reader.IsDBNullAsync(4, token))
            {
                var point = new PointOfInterest
                {
                    Id = PointOfInterestId.From(reader.GetGuid(4)),
                    Name = reader.GetString(5),
                    Duration = new TimeSchedule(reader.GetDouble(6)),
                    PoiType = PoiType.From(reader.GetString(7)),
                    Description = reader.GetString(8),
                    DestinationId = DestinationId.From(reader.GetGuid(9))
                };
                des.AddPoi(point);
            }
        }

        return des;
    }

    public async Task<PointOfInterestId?> AddPoiAsync(PointOfInterest poi, CancellationToken token)
    {
        var sql = @"insert into destination_poi(name, duration, poi_type,description, destination_id) values
                        (@Name,@Duration,@PoiType,@Description,@DestinationId) returning id";
        await using var command = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        command.Parameters.Add(new NpgsqlParameter("@Name", poi.Name));
        command.Parameters.Add(new NpgsqlParameter("@Duration", poi.Duration.Value));
        command.Parameters.Add(new NpgsqlParameter("@PoiType", poi.PoiType.Value));
        command.Parameters.Add(new NpgsqlParameter("@Description", poi.Description));
        command.Parameters.Add(new NpgsqlParameter("@DestinationId", poi.DestinationId));
        var result = await command.ExecuteScalarAsync(token);
        if (result is PointOfInterestId id) return id;
        return null;
    }

    public async Task<IReadOnlyList<Destination>?> GetAllDestinationsAsync(CancellationToken token)
    {
        var sql = "select * from destinations";
        await using var command = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        Dictionary<DestinationId, Destination> destinations = new();
        await using var reader = await command.ExecuteReaderAsync(token);
        while (await reader.ReadAsync(token))
        {
            var id = DestinationId.From(reader.GetGuid(0));
            if (!destinations.ContainsKey(id))
                destinations[id] = new Destination
                {
                    Id = id,
                    Name = reader.GetString(1),
                    Country = reader.GetString(2),
                    IsOverSea = reader.GetBoolean(3)
                };
        }

        return destinations.Values.ToList();
    }


    public async Task<PointOfInterest?> GetPointOfInterestByIdAsync(PointOfInterestId id, CancellationToken token)
    {
        var sql = @"select * from destination_poi where id = @Id";
        await using var command = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        command.Parameters.Add(new NpgsqlParameter("@Id", id.Value));
        await using var reader = await command.ExecuteReaderAsync(token);
        if (await reader.ReadAsync(token))
            return new PointOfInterest
            {
                Id = PointOfInterestId.From(reader.GetGuid(0)),
                Name = reader.GetString(1),
                Duration = new TimeSchedule(reader.GetDouble(2)),
                PoiType = PoiType.From(reader.GetString(3)),
                Description = reader.GetString(4),
                DestinationId = DestinationId.From(reader.GetGuid(7))
            };
        return null;
    }

    public async Task<IReadOnlyList<PointOfInterest>?> GetPointOfInterestByDestinationIdAsync(
        DestinationId destinationId, CancellationToken token)
    {
        var sql = @"select * from destination_poi where destination_id = @Id";
        await using var command = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        command.Parameters.Add(new NpgsqlParameter("@Id", destinationId.Value));
        List<PointOfInterest> list = new();
        await using var reader = await command.ExecuteReaderAsync(token);
        while (await reader.ReadAsync(token))
        {
            var poi = new PointOfInterest
            {
                Id = PointOfInterestId.From(reader.GetGuid(0)),
                Name = reader.GetString(1),
                Duration = new TimeSchedule(reader.GetDouble(2)),
                PoiType = PoiType.From(reader.GetString(3)),
                Description = reader.GetString(4),
                DestinationId = DestinationId.From(reader.GetGuid(7))
            };
            list.Add(poi);
        }

        return list;
    }
}