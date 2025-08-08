using Amazon.Lambda.Core;

namespace EventR.Lambda.Launcher.Services;

public class LocalLambdaContext(string functionName, string functionVersion, int maxMemorySize, ILambdaLogger logger) : ILambdaContext
{
    public string AwsRequestId => string.Empty;

    public IClientContext ClientContext => null!;

    public string FunctionName => functionName;

    public string FunctionVersion => functionVersion;

    public ICognitoIdentity Identity => null!;

    public string InvokedFunctionArn => string.Empty;

    public ILambdaLogger Logger => logger;

    public string LogGroupName => string.Empty;

    public string LogStreamName => string.Empty;

    public int MemoryLimitInMB => maxMemorySize;

    public TimeSpan RemainingTime => TimeSpan.FromMilliseconds(30_000);
}
