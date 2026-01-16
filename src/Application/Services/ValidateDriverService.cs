using Application.DTO;
using Application.Options;
using Application.Ports;
using Application.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class ValidateDriverService : IValidateDriverService
{
    private readonly IValidateDriverPort _validateDriverPort;
    private readonly IOptionsMonitor<DriverSnapshotOptions> _options;

    public ValidateDriverService(IValidateDriverPort validateDriverPort, IOptionsMonitor<DriverSnapshotOptions> options)
    {
        _validateDriverPort = validateDriverPort;
        _options = options;
    }

    public async Task<bool> IsValidDriver(long driverId, CancellationToken cancellationToken)
    {
        return await _validateDriverPort.IsValidDriver(driverId, cancellationToken);
    }

    public bool IsValidSnapshot(DriverSnapshotDto snapshot)
    {
        if (snapshot.Availability != DriverAvailability.Searching)
            return false;

        if (snapshot.Segment is null)
            return false;

        if (DateTime.UtcNow - snapshot.Timestamp > _options.CurrentValue.SnapshotTtl)
            return false;

        return true;
    }
}