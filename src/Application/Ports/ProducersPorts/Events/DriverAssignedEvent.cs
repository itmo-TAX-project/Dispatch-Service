namespace Application.Ports.ProducersPorts.Events;

public class DriverAssignedEvent : IEventMessage
{
    public long RideId { get; set; }

    public long DriverId { get; set; }
}