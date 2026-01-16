using Application.Ports.ProducersPorts;
using Itmo.Dev.Platform.Kafka.Configuration;
using Itmo.Dev.Platform.Kafka.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Kafka.Consumers;
using Presentation.Kafka.Consumers.Keys;
using Presentation.Kafka.Consumers.Values;
using Presentation.Kafka.Producers;
using Presentation.Kafka.Producers.Keys;
using Presentation.Kafka.Producers.Values;

namespace Presentation.Extensions;

public static class KafkaExtensions
{
    public static IServiceCollection AddKafka(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // https://github.com/itmo-is-dev/platform/wiki/Kafka:-Configuration
        services.AddPlatformKafka(builder => builder
            .ConfigureOptions(configuration.GetSection("Kafka"))

            .AddDispatchProducer(configuration)

            .AddRideRequestedConsumer(configuration)
            .AddTaxiDriverStatusChangedConsumer(configuration)
            .AddTaxiDriverVehicleChangedConsumer(configuration));

        services.AddScoped<IDispatchProducer, DispatchProducer>();
        return services;
    }

    private static IKafkaConfigurationBuilder AddDispatchProducer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddProducer(p => p
            .WithKey<RideKey>()
            .WithValue<RideAssignationMessage>()
            .WithConfiguration(configuration.GetSection("Kafka:Producers:DispatchMessage"))
            .SerializeKeyWithNewtonsoft()
            .SerializeValueWithNewtonsoft()
            .WithOutbox());
    }

    private static IKafkaConfigurationBuilder AddRideRequestedConsumer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddConsumer(c => c
            .WithKey<RideConsumerKey>()
            .WithValue<RideRequestedValue>()
            .WithConfiguration(configuration.GetSection("Kafka:Consumers:RideRequestedMessage"))
            .DeserializeKeyWithNewtonsoft()
            .DeserializeValueWithNewtonsoft()
            .HandleInboxWith<RideRequestedConsumer>());
    }

    private static IKafkaConfigurationBuilder AddTaxiDriverStatusChangedConsumer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddConsumer(c => c
            .WithKey<DriverConsumerKey>()
            .WithValue<TaxiDriverStatusChangedValue>()
            .WithConfiguration(configuration.GetSection("Kafka:Consumers:TaxiDriverStatusChangedMessage"))
            .DeserializeKeyWithNewtonsoft()
            .DeserializeValueWithNewtonsoft()
            .HandleInboxWith<TaxiDriverStatusChangedConsumer>());
    }

    private static IKafkaConfigurationBuilder AddTaxiDriverVehicleChangedConsumer(
        this IKafkaConfigurationBuilder builder,
        IConfiguration configuration)
    {
        return builder.AddConsumer(c => c
            .WithKey<DriverConsumerKey>()
            .WithValue<TaxiDriverVehicleChangedValue>()
            .WithConfiguration(configuration.GetSection("Kafka:Consumers:TaxiDriverVehicleChangedMessage"))
            .DeserializeKeyWithNewtonsoft()
            .DeserializeValueWithNewtonsoft()
            .HandleInboxWith<TaxiDriverVehicleChangedConsumer>());
    }
}