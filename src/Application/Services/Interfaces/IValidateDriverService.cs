using Application.DTO;

namespace Application.Services.Interfaces;

public interface IValidateDriverService
{
    Task<bool> IsValidDriver(long driverId, CancellationToken cancellationToken);

    bool IsValidSnapshot(DriverSnapshotDto snapshot);
}