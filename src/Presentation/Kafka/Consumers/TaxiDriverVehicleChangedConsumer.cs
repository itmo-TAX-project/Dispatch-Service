using Application.Services.Interfaces;
using Itmo.Dev.Platform.Kafka.Consumer;
using Presentation.Kafka.Consumers.Keys;
using Presentation.Kafka.Consumers.Values;

namespace Presentation.Kafka.Consumers;

public class TaxiDriverVehicleChangedConsumer : IKafkaInboxHandler<DriverConsumerKey, TaxiDriverVehicleChangedValue>
{
    private readonly ISnapshotDriverService _snapshotDriverService;

    public TaxiDriverVehicleChangedConsumer(ISnapshotDriverService snapshotDriverService)
    {
        _snapshotDriverService = snapshotDriverService;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaInboxMessage<DriverConsumerKey, TaxiDriverVehicleChangedValue>> messages,
        CancellationToken cancellationToken)
    {
        foreach (IKafkaInboxMessage<DriverConsumerKey, TaxiDriverVehicleChangedValue> message in messages)
        {
            TaxiDriverVehicleChangedValue value = message.Value;
            await _snapshotDriverService.SetCurrentVehicleSegmentAsync(value.DriverId, value.Segment, cancellationToken);
        }
    }
}