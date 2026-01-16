using Application.DTO;
using Application.DTO.Enums;

namespace Application.Repositories;

public interface IDriverSnapshotRepository
{
    Task CreateDriverSnapshotAsync(DriverSnapshotDto dto, CancellationToken cancellationToken);

    Task<DriverSnapshotDto?> GetDriverSnapshotAsync(long driverId, CancellationToken cancellationToken);

    Task UpdateDriverSnapshotAsync(DriverSnapshotDto dto, CancellationToken cancellationToken);

    Task SetCurrentVehicleSegmentAsync(long driverId, VehicleSegment segment, CancellationToken cancellationToken);

    Task DeleteDriverSnapshotAsync(long driverId, CancellationToken cancellationToken);

    IAsyncEnumerable<DriverSnapshotDto> GetAllAsync(CancellationToken cancellationToken);
}