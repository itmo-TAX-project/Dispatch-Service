namespace Application.Options;

public class DriverSnapshotOptions
{
    public TimeSpan SnapshotTtl { get; init; } = TimeSpan.FromSeconds(30);
}