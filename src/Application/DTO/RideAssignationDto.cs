namespace Application.DTO;

public class RideAssignationDto
{
    public long RideId { get; set; }

    public long PassengerId { get; set; }

    public required PointDto PickupLocation { get; set; }
}