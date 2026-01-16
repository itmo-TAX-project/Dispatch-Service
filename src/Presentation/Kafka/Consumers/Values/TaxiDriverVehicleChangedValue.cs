using Application.DTO.Enums;

namespace Presentation.Kafka.Consumers.Values;

public class TaxiDriverVehicleChangedValue
{
    public long DriverId { get; set; }

    public long VehicleId { get; set; }

    public VehicleSegment Segment { get; set; }
}