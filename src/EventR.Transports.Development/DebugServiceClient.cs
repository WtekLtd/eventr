using System.Net;
using System.Net.Http.Json;
using EventR.Cli.Interface.Requests;
using EventR.Cli.Interface.Responses;

namespace EventR.Transports.Development;

public class DebugServiceClient(IHttpClientFactory httpClientFactory) : IDebugServiceClient
{
    public const string ClientName = "eventr.debugger";

    public async Task<GetEventResponse?> GetEventAsync(string endpointIdentifier, CancellationToken cancellationToken)
    {
        var httpClient = httpClientFactory.CreateClient(ClientName);

        var url = $"endpoints/{endpointIdentifier}/events";
        var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GetEventResponse>(cancellationToken);
    }

    public async Task PublishEventAsync(string endpointIdentifier, PublishEventRequest request, CancellationToken cancellationToken)
    {
        var httpClient = httpClientFactory.CreateClient(ClientName);

        var uri = $"endpoints/{endpointIdentifier}/events";
        var body = JsonContent.Create(request);
        var response = await httpClient.PostAsync(uri, body, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task PublishLogAsync(string endpointIdentifier, PublishLogRequest request, CancellationToken cancellationToken)
    {
        var httpClient = httpClientFactory.CreateClient(ClientName);

        try
        {
            var uri = $"endpoints/{endpointIdentifier}/logs";
            var body = JsonContent.Create(request);
            var response = await httpClient.PostAsync(uri, body, cancellationToken);
        }
        catch (HttpRequestException)
        {
            // Ignore connection errors. service is not yet available.
        }
    }

    public async Task<bool> RegisterEndpointAsync(RegisterEndpointRequest request, CancellationToken cancellationToken)
    {
        var httpClient = httpClientFactory.CreateClient(ClientName);

        HttpStatusCode? statusCode;
        try
        {
            var uri = "endpoints";
            var body = JsonContent.Create(request);
            var response = await httpClient.PostAsync(uri, body, cancellationToken);
            statusCode = response.StatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }


        if (statusCode != HttpStatusCode.OK)
        {
            throw new HttpRequestException($"Failed to register endpoint. Service returned code {statusCode}");
        }

        return true;
    }

    public async Task DetachEndpointAsync(string endpointIdentifier, CancellationToken cancellationToken)
    {
        var httpClient = httpClientFactory.CreateClient(ClientName);

        var uri = $"endpoints/{endpointIdentifier}";
        var response = await httpClient.DeleteAsync(uri, cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateEventStatusAsync(string eventIdentifier, UpdateEventStatusRequest request, CancellationToken cancellationToken)
    {
        var httpClient = httpClientFactory.CreateClient(ClientName);

        var uri = $"events/{eventIdentifier}";
        var body = JsonContent.Create(request);
        var response = await httpClient.PutAsync(uri, body, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}