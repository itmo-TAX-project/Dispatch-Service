namespace Application.Ports;

public interface IValidateDriverPort
{
    Task<bool> IsValidDriver(long driverId, CancellationToken cancellationToken);
}