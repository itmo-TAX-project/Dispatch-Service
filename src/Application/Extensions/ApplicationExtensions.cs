using Application.Options;
using Application.Services;
using Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDispatchService, DispatchService>();
        services.AddScoped<ISnapshotDriverService, SnapshotDriverService>();
        services.AddScoped<IValidateDriverService, ValidateDriverService>();

        services.Configure<DriverSnapshotOptions>(configuration.GetSection("DriverSnapshot"));
        return services;
    }
}