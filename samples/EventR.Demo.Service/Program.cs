using EventR;
using EventR.Demo.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddEventR((eventr) =>
{
    if (builder.Environment.IsDevelopment())
    {
        eventr.UseDebugger(config =>
        {
            config.Port = 5050;

            config.Identifier = "EventR.Demo.Service";
            config.Name = "Demo Service";
            config.SavedEventPath = ".eventr";
            config.Columns = [
                new() { Title = "Detail Type", Pointer = "/detail-type" }
            ];
        });
    }
    else
    {
        eventr
            .UseAmazonSQS()
                .AsSource(config =>
                {
                    config.MaxNumberOfMessages = 10;
                    config.QueueUrl = "https://sqs.us-east-1.amazonaws.com/123456789012/MyQueue";
                    config.VisibilityTimeout = 30;
                    config.WaitTimeSeconds = 20;
                })
            .UseAmazonEventBridge()
                .AsProducer(config =>
                {
                    config.EventBusName = "MyEventBus";
                });
    }
    eventr.AddEventProcessor<DemoEventProcessor>();
});

var app = builder.Build();

await app.RunAsync();