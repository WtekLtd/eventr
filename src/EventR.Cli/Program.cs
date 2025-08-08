using System.CommandLine;
using EventR.Cli.Commands;

var rootCommand = new RootCommand("EventR CLI.");

rootCommand.AddDebuggerCommand();
rootCommand.AddConfigCommand();

await rootCommand.InvokeAsync(args);