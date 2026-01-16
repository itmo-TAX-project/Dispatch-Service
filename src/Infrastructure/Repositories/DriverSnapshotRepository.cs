using Application.DTO;
using Application.DTO.Enums;
using Application.Repositories;
using Npgsql;
using System.Runtime.CompilerServices;

namespace Infrastructure.Repositories;

public class DriverSnapshotRepository : IDriverSnapshotRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public DriverSnapshotRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task CreateDriverSnapshotAsync(DriverSnapshotDto dto, CancellationToken cancellationToken)
    {
        const string sql = """
                           INSERT INTO driver_snapshots (
                               driver_id,
                               latitude,
                               longitude,
                               availability,
                               segment,
                               ts
                           )
                           VALUES (
                               @driver_id,
                               @latitude,
                               @longitude,
                               @availability,
                               @segment,
                               @ts
                           );
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("driver_id", dto.DriverId);
        command.Parameters.AddWithValue("latitude", dto.Latitude);
        command.Parameters.AddWithValue("longitude", dto.Longitude);
        command.Parameters.AddWithValue("availability", dto.Availability);
        command.Parameters.AddWithValue("segment", dto.Segment is null ? DBNull.Value : dto.Segment);
        command.Parameters.AddWithValue("ts", dto.Timestamp);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<DriverSnapshotDto?> GetDriverSnapshotAsync(long driverId, CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT *
                           FROM driver_snapshots
                           WHERE driver_id = @driver_id;
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("driver_id", driverId);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return new DriverSnapshotDto
        {
            DriverId = reader.GetInt64(0),
            Latitude = reader.GetDouble(1),
            Longitude = reader.GetDouble(2),
            Availability = await reader.GetFieldValueAsync<DriverAvailability>(3, cancellationToken),
            Segment = await reader.IsDBNullAsync(4, cancellationToken) ? null
                : await reader.GetFieldValueAsync<VehicleSegment>(4, cancellationToken),
            Timestamp = reader.GetDateTime(5),
        };
    }

    public async Task UpdateDriverSnapshotAsync(DriverSnapshotDto dto, CancellationToken cancellationToken)
    {
        const string sql = """
                           UPDATE driver_snapshots
                           SET
                               latitude = @latitude,
                               longitude = @longitude,
                               availability = @availability,
                               segment = @segment,
                               ts = @ts
                           WHERE driver_id = @driver_id;
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("driver_id", dto.DriverId);
        command.Parameters.AddWithValue("latitude", dto.Latitude);
        command.Parameters.AddWithValue("longitude", dto.Longitude);
        command.Parameters.AddWithValue("availability", dto.Availability);
        command.Parameters.AddWithValue("segment", dto.Segment is null ? DBNull.Value : dto.Segment);
        command.Parameters.AddWithValue("ts", dto.Timestamp);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task SetCurrentVehicleSegmentAsync(long driverId, VehicleSegment segment, CancellationToken cancellationToken)
    {
        const string sql = """
                           UPDATE driver_snapshots
                           SET segment = @segment
                           WHERE driver_id = @driver_id;
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("driver_id", driverId);
        command.Parameters.AddWithValue("segment", segment);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task DeleteDriverSnapshotAsync(long driverId, CancellationToken cancellationToken)
    {
        const string sql = """
                           DELETE FROM driver_snapshots
                           WHERE driver_id = @driver_id;
                           """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("driver_id", driverId);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async IAsyncEnumerable<DriverSnapshotDto> GetAllAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
                        SELECT *
                        FROM driver_snapshots;
                        """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new DriverSnapshotDto
            {
                DriverId = reader.GetInt64(0),
                Latitude = reader.GetDouble(1),
                Longitude = reader.GetDouble(2),
                Availability = await reader.GetFieldValueAsync<DriverAvailability>(3, cancellationToken),
                Segment = await reader.IsDBNullAsync(4, cancellationToken) ? null
                    : await reader.GetFieldValueAsync<VehicleSegment>(4, cancellationToken),
                Timestamp = reader.GetDateTime(5),
            };
        }
    }
}