# Amazon SQS Transport

[EventR](https://github.com/WtekLtd/eventr) provides users the ability to retrieve events from, and publish onward events to an Amazon SQS queue.

## Installing

The SQS transport is available as a nuget package. To install, run the following command...

```bash
dotnet add package EventR.Transports.Amazon.SQS
```

## Adding to a service as a source and producer.

To add the SQS producer and source to your service modify the AddEventR method as follows...

```cs
builder.Services.AddDefaultAWSOptions(builder.Configuation.GetAWSOptions());
builder.Services.AddEventR(eventr => {
    eventr.UseSQS()
        .AsProducer(config =>
        {
            config.QueueUrl = "https://sqs.us-east-1.amazonaws.com/123456789012/MyDestinationQueue";
        })
        .AsSource(config => 
        {
            config.MaxNumberOfMessages = 10;
            config.QueueUrl = "https://sqs.us-east-1.amazonaws.com/123456789012/MySourceQueue";
            config.VisibilityTimeout = 30;
            config.WaitTimeSeconds = 20;
        });

    // OR

    eventR.SQS()
        .AsProducer(builder.Services.Configuration.GetSection("SQS:Destination"))
        .AsSource(builder.Services.Configuration.GetSection("SQS:Source"));
});
```