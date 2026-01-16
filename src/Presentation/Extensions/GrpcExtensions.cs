using Application.Ports;
using Dispatches.Grpc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Grpc.ClientServices;

namespace Presentation.Extensions;

public static class GrpcExtensions
{
    public static IServiceCollection AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
    {
        string routeServiceAddress =
            configuration.GetValue<string>("Grpc:TaxiServiceAddress")
            ?? throw new InvalidOperationException("Grpc:TaxiServiceAddress is not configured");

        services.AddGrpcClient<TaxiService.TaxiServiceClient>(options =>
        {
            options.Address = new Uri(routeServiceAddress);
        });
        services.AddScoped<IValidateDriverPort, ValidateDriverActiveService>();

        return services;
    }
}