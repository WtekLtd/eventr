using Amazon.SQS;
using EventR.Contracts;
using EventR.Transports.Amazon.SQS;
using EventR.Transports.Amazon.SQS.Producer;
using EventR.Transports.Amazon.SQS.Source;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventR;

public static class SQSConfigurationExtensions
{
    public static EventRSQSConfigurationBuilder UseAmazonSQS(this EventRConfigurationBuilder builder)
    {
        builder.Services.AddAWSService<IAmazonSQS>();

        return new(builder.Services);
    }

    public static EventRSQSConfigurationBuilder AsSource(this EventRSQSConfigurationBuilder builder, IConfiguration config)
    {
        builder.Services
            .AddOptions<SQSEventSourceOptions>()
            .Bind(config)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        builder.AddSQSSourceServices();
        return builder;
    }

    public static EventRSQSConfigurationBuilder AsSource(this EventRSQSConfigurationBuilder builder, Action<SQSEventSourceOptions> configure)
    {
        builder.Services
            .AddOptions<SQSEventSourceOptions>()
            .Configure(configure)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        builder.AddSQSSourceServices();
        return builder;
    }

    public static EventRSQSConfigurationBuilder AsProducer(this EventRSQSConfigurationBuilder builder, IConfiguration config)
    {
        builder.Services
            .AddOptions<SQSEventProducerOptions>()
            .Bind(config)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        builder.AddSQSProducerServices();
        return builder;
    }

    public static EventRSQSConfigurationBuilder AsProducer(this EventRSQSConfigurationBuilder builder, Action<SQSEventProducerOptions> configure)
    {
        builder.Services
            .AddOptions<SQSEventProducerOptions>()
            .Configure(configure)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        builder.AddSQSProducerServices();
        return builder;
    }

    private static void AddSQSSourceServices(this EventRSQSConfigurationBuilder builder)
    {
        builder.Services.AddSingleton<IEventSource, SQSEventSource>();
    }

    private static void AddSQSProducerServices(this EventRSQSConfigurationBuilder builder)
    {
        builder.Services.AddSingleton<IEventProducer, SQSEventProducer>();
    }
}
