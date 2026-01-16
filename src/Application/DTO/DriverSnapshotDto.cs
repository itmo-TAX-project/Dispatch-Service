using Application.DTO.Enums;

namespace Application.DTO;

public class DriverSnapshotDto
{
    public long DriverId { get; init; }

    public double Latitude { get; init; }

    public double Longitude { get; init; }

    public DriverAvailability Availability { get; init; }

    public VehicleSegment? Segment { get; init; }

    public DateTime Timestamp { get; init; }
}