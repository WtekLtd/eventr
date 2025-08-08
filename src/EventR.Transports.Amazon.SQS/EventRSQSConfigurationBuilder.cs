using Microsoft.Extensions.DependencyInjection;

namespace EventR.Transports.Amazon.SQS;

public class EventRSQSConfigurationBuilder(IServiceCollection services)
    : EventRConfigurationBuilder(services);