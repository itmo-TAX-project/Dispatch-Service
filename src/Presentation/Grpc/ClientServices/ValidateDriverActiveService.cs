using Application.Ports;
using Dispatches.Grpc;

namespace Presentation.Grpc.ClientServices;

public class ValidateDriverActiveService : IValidateDriverPort
{
    private readonly TaxiService.TaxiServiceClient _taxiServiceClient;

    public ValidateDriverActiveService(TaxiService.TaxiServiceClient taxiServiceClient)
    {
        _taxiServiceClient = taxiServiceClient;
    }

    public async Task<bool> IsValidDriver(long driverId, CancellationToken cancellationToken)
    {
        ValidateDriverResponse response = await _taxiServiceClient.ValidateDriverActiveAsync(
            new ValidateDriverRequest { DriverId = driverId },
            cancellationToken: cancellationToken);
        return response.IsActive;
    }
}