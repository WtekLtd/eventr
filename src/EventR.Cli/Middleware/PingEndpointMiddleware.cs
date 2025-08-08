using EventR.Cli.Requests.Commands.PingEndpoint;
using EventR.Cli.Services.RequestDispatch;

namespace EventR.Cli.Middleware;

public class PingEndpointMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IRequestDispatcher requestDispatcher)
    {
        if (context.Request.Path.HasValue)
        {
            var pathSegments = context.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (pathSegments is ["api", "endpoints", var endpointIdentifier, ..])
            {
                requestDispatcher.DispatchCommand(new PingEndpointCommand
                {
                    EndpointIdentifier = endpointIdentifier
                });
            }
        }

        await next(context);
    }
}