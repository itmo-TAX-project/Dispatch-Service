using Application.Ports.ProducersPorts;
using Application.Ports.ProducersPorts.Events;
using Itmo.Dev.Platform.Kafka.Extensions;
using Itmo.Dev.Platform.Kafka.Producer;
using Presentation.Kafka.Producers.Keys;
using Presentation.Kafka.Producers.Values;

namespace Presentation.Kafka.Producers;

public class DispatchProducer : IDispatchProducer
{
    private readonly IKafkaMessageProducer<RideKey, RideAssignationMessage> _rideAssignationProducer;

    public DispatchProducer(IKafkaMessageProducer<RideKey, RideAssignationMessage> rideAssignationProducer)
    {
        _rideAssignationProducer = rideAssignationProducer;
    }

    public async Task ProduceAsync(DriverAssignationFailedEvent assignationFailedEvent, CancellationToken cancellationToken)
    {
        var key = new RideKey { RideId = assignationFailedEvent.RideId };
        var value = new RideAssignationMessage() { DriverId = -1, AssignationStatus = AssignationStatus.Failed };
        var message = new KafkaProducerMessage<RideKey, RideAssignationMessage>(key, value);

        await _rideAssignationProducer.ProduceAsync(message, cancellationToken);
    }

    public async Task ProduceAsync(DriverAssignedEvent driverAssignedEvent, CancellationToken cancellationToken)
    {
        var key = new RideKey { RideId = driverAssignedEvent.RideId };
        var value = new RideAssignationMessage
        {
            DriverId = driverAssignedEvent.DriverId,
            AssignationStatus = AssignationStatus.Failed,
        };
        var message = new KafkaProducerMessage<RideKey, RideAssignationMessage>(key, value);

        await _rideAssignationProducer.ProduceAsync(message, cancellationToken);
    }
}