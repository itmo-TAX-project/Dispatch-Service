using Application.DTO;

namespace Presentation.Kafka.Consumers.Values;

public class TaxiDriverStatusChangedValue
{
    public long DriverId { get; init; }

    public double Latitude { get; init; }

    public double Longitude { get; init; }

    public DriverAvailability Availability { get; init; }

    public DateTime Timestamp { get; init; }
}