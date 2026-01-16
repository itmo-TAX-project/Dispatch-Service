using Application.Ports.ProducersPorts.Events;

namespace Application.Ports.ProducersPorts;

public interface IDispatchProducer
{
    Task ProduceAsync(DriverAssignationFailedEvent assignationFailedEvent, CancellationToken cancellationToken);

    Task ProduceAsync(DriverAssignedEvent driverAssignedEvent, CancellationToken cancellationToken);
}