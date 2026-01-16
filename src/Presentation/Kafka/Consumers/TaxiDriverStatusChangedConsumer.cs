using Application.DTO;
using Application.Services.Interfaces;
using Itmo.Dev.Platform.Kafka.Consumer;
using Presentation.Kafka.Consumers.Keys;
using Presentation.Kafka.Consumers.Values;

namespace Presentation.Kafka.Consumers;

public class TaxiDriverStatusChangedConsumer : IKafkaInboxHandler<DriverConsumerKey, TaxiDriverStatusChangedValue>
{
    private readonly ISnapshotDriverService _snapshotDriverService;

    public TaxiDriverStatusChangedConsumer(ISnapshotDriverService snapshotDriverService)
    {
        _snapshotDriverService = snapshotDriverService;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaInboxMessage<DriverConsumerKey, TaxiDriverStatusChangedValue>> messages,
        CancellationToken cancellationToken)
    {
        foreach (IKafkaInboxMessage<DriverConsumerKey, TaxiDriverStatusChangedValue> message in messages)
        {
            TaxiDriverStatusChangedValue value = message.Value;
            var dto = new DriverSnapshotDto
            {
                DriverId = value.DriverId,
                Latitude = value.Latitude,
                Longitude = value.Longitude,
                Availability = value.Availability,
                Timestamp = value.Timestamp,
            };
            await _snapshotDriverService.AddDriverSnapshotAsync(dto, cancellationToken);
        }
    }
}