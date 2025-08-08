# Amazon EventBridge Transport

[EventR](ttps://github.com/WtekLtd/eventr) provides users the ability to publish onward events to an Amazon EventBridge bus. Due to the nature of EventBridge being a "push" style system, we do not provide this transport as an event source.

## Installing

The EventBridge transport is available as a nuget package. To install, run the following command...

```bash
dotnet add package EventR.Transports.Amazon.EventBridge
```

## Adding to a service as a producer.

To add the EventBridge producer to your service modify your Program.cs as follows...

```cs
builder.Services.AddDefaultAWSOptions(builder.Configuation.GetAWSOptions());
builder.Services.AddEventR(eventr => {
    eventr.UseEventBridge()
        .AsProducer(eb => 
        {
            eb.EventBusName = "myEventBus"
        });

    // OR

    eventR.UseEventBridge()
        .AsProducer(builder.Configuration.GetSection("EventBridge"));
});
```