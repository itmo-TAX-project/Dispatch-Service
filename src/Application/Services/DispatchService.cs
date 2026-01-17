using Application.DTO;
using Application.Ports;
using Application.Ports.ProducersPorts;
using Application.Ports.ProducersPorts.Events;
using Application.Repositories;
using Application.Services.Interfaces;

namespace Application.Services;

public class DispatchService : IDispatchService
{
    private readonly IDriverSnapshotRepository _snapshotRopository;
    private readonly IDistanceCalculator _distanceCalculator;
    private readonly IDispatchProducer _producer;
    private readonly IValidateDriverService _validateDriverService;

    public DispatchService(
        IDriverSnapshotRepository snapshotRopository,
        IDistanceCalculator distanceCalculator,
        IDispatchProducer producer,
        IValidateDriverService validateDriverService)
    {
        _snapshotRopository = snapshotRopository;
        _distanceCalculator = distanceCalculator;
        _producer = producer;
        _validateDriverService = validateDriverService;
    }

    public async Task AssignDriver(RideAssignationDto dto, CancellationToken cancellationToken)
    {
        DriverSnapshotDto? bestCandidate = null;
        double bestDistance = double.MaxValue;

        await foreach (DriverSnapshotDto snapshot in _snapshotRopository.GetAllAsync(cancellationToken))
        {
            if (!_validateDriverService.IsValidSnapshot(snapshot))
                continue;

            double distance = _distanceCalculator.CalculateMeters(
                new PointDto(snapshot.Latitude, snapshot.Longitude),
                new PointDto(dto.PickupLocation.Latitude, dto.PickupLocation.Longitude));

            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestCandidate = snapshot;
            }
        }

        if (bestCandidate is null ||
            !await _validateDriverService.IsValidDriver(bestCandidate.DriverId, cancellationToken))
        {
            var assignationFailedEvent = new DriverAssignationFailedEvent
            {
                RideId = dto.RideId,
            };
            await _producer.ProduceAsync(assignationFailedEvent, cancellationToken);
            return;
        }

        var driverAssignedEvent = new DriverAssignedEvent
        {
            RideId = dto.RideId,
            DriverId = bestCandidate.DriverId,
        };
        await _producer.ProduceAsync(driverAssignedEvent, cancellationToken);
    }
}