namespace Application.Ports.ProducersPorts.Events;

public class DriverAssignationFailedEvent : IEventMessage
{
    public long RideId { get; set; }
}