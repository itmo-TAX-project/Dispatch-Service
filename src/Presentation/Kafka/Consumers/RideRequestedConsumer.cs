using Application.DTO;
using Application.Services.Interfaces;
using Itmo.Dev.Platform.Kafka.Consumer;
using Presentation.Kafka.Consumers.Keys;
using Presentation.Kafka.Consumers.Values;

namespace Presentation.Kafka.Consumers;

public class RideRequestedConsumer : IKafkaInboxHandler<RideConsumerKey, RideRequestedValue>
{
    private readonly IDispatchService _dispatchService;

    public RideRequestedConsumer(IDispatchService dispatchService)
    {
        _dispatchService = dispatchService;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaInboxMessage<RideConsumerKey, RideRequestedValue>> messages,
        CancellationToken cancellationToken)
    {
        foreach (IKafkaInboxMessage<RideConsumerKey, RideRequestedValue> message in messages)
        {
            RideRequestedValue value = message.Value;
            var dto = new RideAssignationDto
            {
                PassengerId = value.PassengerId,
                RideId = value.RideId,
                PickupLocation = value.PickupLocation,
            };

            await _dispatchService.AssignDriver(dto, cancellationToken);
        }
    }
}