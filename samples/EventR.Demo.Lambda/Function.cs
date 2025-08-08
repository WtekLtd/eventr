using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EventR.Demo.Lambda;

public class Function
{
    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    public Function()
    {

    }


    /// <summary>
    /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
    /// to respond to SQS messages.
    /// </summary>
    /// <param name="evnt">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task<ResponseEvent> FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        var response = new ResponseEvent();
        foreach (var message in evnt.Records)
        {
            var messageText = await ProcessMessageAsync(message, context);
            response.Messages.Add(messageText);
        }
        return response;
    }

    private static Task<string> ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
    {
        var messageText = $"Processed message {message.Body}";

        context.Logger.LogInformation(messageText);

        return Task.FromResult(messageText);
    }
}

public class ResponseEvent
{
    public ICollection<string> Messages { get; } = [];
}