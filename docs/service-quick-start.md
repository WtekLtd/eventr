# Service Quick Start

## Installing the cli.

The EventR CLI is available on nuget as a dotnet package. To install run the following command:

```bash
dotnet tool install --global EventR.Cli
```

If you haven't added the global tools directory to your PATH environment variable, follow the instructions provided by the dotnet command above. The cli should now be available by calling eventr from the command line.

## Creating a new debuggable service.

EventR integrates with the dotnet generic hosting platform, so it can be added to either a standalone service or ASP.NET Core web application.

For this quickstart we will use a simple console app.

### Create a new console project

```bash
dotnet new console -n "EventRQuickStart"
cd EventRQuickStart
```

### Add EventR package

```bash
dotnet add package EventR.Transports.Development
```

### Define an event processor class to handle your received events.

```csharp
public class DemoEventProcessor(ILogger<DemoEventProcessor> logger) : IEventProcessor
{
  public async Task ProcessEventAsync(IEventProcessorContext context, CancellationToken cancellationToken)
  {
    logger.LogInformation(context.IncomingEvent.Data);
  }
}
```

### Create a new application builder in Program.cs and add EventR

```csharp
var builder = Host.CreateApplicationBuilder();

builder.Services.AddEventR(eventr => {
  if (builder.Environment.IsDevelopment())
  {
    eventr.UseDebugger(config =>
    {
      config.Port = 5050;

      config.Identifier = "UNIQUE SERVICE IDENTIFIER";
      config.Name = "My Service";
      config.SavedEventPath = ".eventr";
      config.Columns = [
          new() { Title = "Detail Type", Pointer = "/detail-type" }
      ];
    });
  }
  else
  {
    // TODO: Here we will add transport configuration later.
  }
  eventr.AddEventProcessor<DemoEventProcessor>();
});

var app = builder.Build();

await app.RunAsync();
```

## Starting the debugger.

To start the debugger service, run the following command in a terminal. You only need to do this once, all debugged services will connect to the same debugger instance.

```bash
eventr debugger start
```

The debugger will start up on the default port and the default web browser will automatically open.

## Start debugging!

You are now ready to start up your new app! Start debugging the app in your favourite IDE and you should see the service appear in the open debugger window.

## Next Steps

1) Learn how to [use the debugger](./using-the-debugger.md) app to send events between apps.
2) Add a [transport](./transports/transports.md) to your app to use when the debugger is not attached.