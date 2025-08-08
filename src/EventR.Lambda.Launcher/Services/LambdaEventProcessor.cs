using System.Reflection;
using System.Text;
using Amazon.Lambda.Core;
using EventR.Contracts;
using EventR.Lambda.Launcher.Utils;
using EventR.Transports.Development;
using EventR.Transports.Development.Configuration;
using Microsoft.Extensions.Options;

namespace EventR.Lambda.Launcher.Services;

public class LambdaEventProcessorOptions
{
    public required Assembly Assembly { get; set; }

    public required string? FunctionHandler { get; set; }

    public required string? FunctionName { get; set; }

    public required string? FunctionVersion { get; set; }

    public required int? MaxMemorySize { get; set; }
}

public class LambdaEventProcessor(
    IOptions<LambdaEventProcessorOptions> options,
    IOptions<DevelopmentTransportOptions> debugOptions,
    IDebugServiceClient debugServiceClient) : IEventProcessor
{
    public async Task ProcessEventAsync(IEventProcessorContext context, CancellationToken cancellationToken)
    {
        var assembly = options.Value.Assembly;

        var functionHandler = options.Value.FunctionHandler;
        if (string.IsNullOrWhiteSpace(functionHandler))
        {
            throw new InvalidDataException("Function handler required.");
        }

        var functionHandlerParts = functionHandler.Split("::");
        if (functionHandlerParts.Length != 3)
        {
            throw new InvalidDataException($"Invalid function handler {functionHandler}");
        }

        var typeName = functionHandlerParts[1];
        var methodName = functionHandlerParts[2];

        var type = assembly.GetType(typeName);
        if (type == null)
        {
            throw new InvalidDataException($"Unable to find type {typeName} on assembly {assembly.FullName}");
        }

        var method = type.GetMethod(methodName);
        if (method == null)
        {
            throw new InvalidDataException($"Unable to find method {methodName} on type {type.FullName}");
        }

        var methodParameters = method.GetParameters();
        if (methodParameters.Length > 2)
        {
            throw new InvalidDataException($"Handler has an invalid number of parameters (expected 2, actual {methodParameters.Length})");
        }

        var serializerAttribute = method.GetCustomAttribute<LambdaSerializerAttribute>()
            ?? assembly.GetCustomAttribute<LambdaSerializerAttribute>();
        if (serializerAttribute == null)
        {
            throw new InvalidDataException($"No lambda serializer specified.");
        }

        var serializerType = serializerAttribute.SerializerType;
        var serializer = Activator.CreateInstance(serializerType) as ILambdaSerializer;
        if (serializer == null)
        {
            throw new InvalidDataException($"Invalid serializer type {serializerType.FullName}");
        }

        var deserializeMethod = serializerType.GetMethod(nameof(ILambdaSerializer.Deserialize))!;
        var serializeMethod = serializerType.GetMethod(nameof(ILambdaSerializer.Serialize))!;

        var parameterValues = new object?[methodParameters.Length];
        for (var i = 0; i < methodParameters.Length; i++)
        {
            var methodParameter = methodParameters[i];

            if (methodParameter.ParameterType == typeof(ILambdaContext))
            {
                var logger = new LocalLambdaLogger(debugServiceClient, debugOptions.Value.Identifier);
                parameterValues[i] = new LocalLambdaContext(
                    functionName: options.Value.FunctionName ?? string.Empty,
                    functionVersion: options.Value.FunctionVersion ?? string.Empty,
                    maxMemorySize: options.Value.MaxMemorySize ?? 0,
                    logger);
            }
            else
            {
                var eventBytes = Encoding.UTF8.GetBytes(context.IncomingEvent.Data);
                using var eventStream = new MemoryStream(eventBytes);

                parameterValues[i] = deserializeMethod
                    .MakeGenericMethod(methodParameter.ParameterType)
                    .Invoke(serializer, [eventStream]);
            }
        }

        var handler = Activator.CreateInstance(type);
        var response = await method.Invoke(handler, parameterValues).AsTask();

        if (response != null)
        {
            using var stream = new MemoryStream();
            serializeMethod
                .MakeGenericMethod(response.GetType())
                .Invoke(serializer, [response, stream]);

            var responseBytes = stream.ToArray();
            var responseText = Encoding.UTF8.GetString(responseBytes);

            context.OutgoingEvents.Add(new()
            {
                Data = responseText,
                Type = "Event"
            });
        }
    }
}
