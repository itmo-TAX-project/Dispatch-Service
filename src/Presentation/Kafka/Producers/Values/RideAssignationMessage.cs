namespace Presentation.Kafka.Producers.Values;

public class RideAssignationMessage
{
    public AssignationStatus AssignationStatus { get; set; }

    public long DriverId { get; set; }
}