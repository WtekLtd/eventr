# Development Transport

The [EventR](https://github.com/WtekLtd/eventr) development transport contains the source, producer and logger required to connect an application to the debugger tool.

## Installing

The Development transport is available as a nuget package. To install, run the following command...

```bash
dotnet add package EventR.Transports.Development
```

## Adding to a service as a source and producer.

To add the Development producer, source and logger to your service modify the AddEventR method as follows...

```cs
builder.Services.AddEventR(eventr => {
    if (builder.Environment.IsDevelopment())
    {
        eventr.UseDebugger((config) =>
        {
            config.Port = 5050;
            config.Endpoint = new()
            {
                Identifier = "abcc5e49-7c79-499e-bb42-2dc052dccfcd",
                Name = "My Application",
                SavedEventPath = ".eventr",
                Columns = []
            };
        });
    }
});
```