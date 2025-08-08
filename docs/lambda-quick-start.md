# AWS Lambda Quick Start

## Installing the CLI.

The EventR CLI is available on nuget as a dotnet package. To install run the following command:

```bash
dotnet tool install --global EventR.Cli
```

If you haven't added the global tools directory to your PATH environment variable, follow the instructions provided by the above command. The cli should now be available by calling eventr from the command line.

## Install the Amazon lambda tools / templates.

EventR integrates with AWS class library handlers. Top level functions are not yet supported.

To install the latest version of the templates execute the following command...

```bash
dotnet tool install --global Amazon.Lambda.Tools
```

or to upgrade to the latest version run...

```bash
dotnet tool update --global Amazon.Lambda.Tools
```

## Create a new lambda project.

```bash
dotnet new lambda.EmptyFunction  -n "EventRQuickStartLambda"
cd EventRQuickStartLambda
```

## Install the lambda launcher.

For lambdas, instead of including EventR in the project code, you debug the lambda using a dotnet tool. To install the tool locally run the following command:

```bash
dotnet new tool-manifest
dotnet tool install EventR.Lambda.Launcher
```

This will install the tool locally. Future developers can run the following command to restore the latest version of the tool:

```bash
dotnet tool restore
```

### Configure the debugger.

In order to debug your application using visual studio (or visual studio code using the C# Dev Kit extension) copy the below json into your ./Properties/launchsettings.json file. Change ```[SERVICE_NAME]``` to the display name of your service as it should appear in the debugger UI.

```json
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  
  "profiles": {
    "EventR": {
      "commandName": "Executable",
      "commandLineArgs": "eventr-lambda-launcher --port 5050 --name \"[SERVICE_NAME]\" $(TargetPath)",
      "executablePath": "dotnet",
      "workingDirectory": "$(ProjectDir)",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

## Start debugging!

You are now ready to start up your new app! Start debugging the app in your favourite IDE and you should see the service appear in the open debugger window.

## Next Steps

1) Learn how to [use the debugger](https://github.com/WtekLtd/eventr/blob/main/docs/using-the-debugger.md) app to send events between apps.
2) Add a [transport](https://github.com/WtekLtd/eventr/blob/main/docs/transports/transports.md) to your app to use when the debugger is not attached.