using Application.DTO;
using Application.DTO.Enums;
using Itmo.Dev.Platform.Persistence.Postgres.Plugins;
using Npgsql;

namespace Infrastructure.Extensions.Plugins;

public class MappingPlugin : IPostgresDataSourcePlugin
{
    public void Configure(NpgsqlDataSourceBuilder dataSource)
    {
        dataSource.MapEnum<VehicleSegment>("vehicle_segment");
        dataSource.MapEnum<DriverAvailability>("ride_status");
    }
}