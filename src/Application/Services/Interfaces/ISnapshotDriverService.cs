using Application.DTO;
using Application.DTO.Enums;

namespace Application.Services.Interfaces;

public interface ISnapshotDriverService
{
    Task AddDriverSnapshotAsync(DriverSnapshotDto dto, CancellationToken cancellationToken);

    Task UpdateDriverSnapshotAsync(DriverSnapshotDto dto, CancellationToken cancellationToken);

    Task SetCurrentVehicleSegmentAsync(long driverId, VehicleSegment segment, CancellationToken cancellationToken);

    Task DeleteDriverSnapshotAsync(long driverId, CancellationToken cancellationToken);
}