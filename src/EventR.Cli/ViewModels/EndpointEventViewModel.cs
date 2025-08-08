using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using EventR.Cli.Constants;
using EventR.Cli.DTOs;
using Json.Pointer;

namespace EventR.Cli.ViewModels;

public class EndpointEventViewModel(EndpointEventDto endpointEvent) : INotifyPropertyChanged
{
    private static readonly JsonSerializerOptions IndentedJsonSerializerOptions = new()
    {
        WriteIndented = true,
    };

    public string EventIdentifier => endpointEvent.Identifier;

    public string EndpointName => endpointEvent.EndpointName;

    public string? Data => endpointEvent.Data;

    public string FormattedData
    {
        get
        {
            if (Data == null)
            {
                return string.Empty;
            }

            try
            {
                using var dataJson = JsonDocument.Parse(Data);
                var indentedJson = JsonSerializer.Serialize(
                    value: dataJson.RootElement,
                    options: IndentedJsonSerializerOptions);

                return indentedJson;

            }
            catch (JsonException)
            {
                return Data ?? string.Empty;
            }
        }
    }

    public DateTime DateTime => endpointEvent.DateTime;

    public EventStatus Status { get; private set; } = endpointEvent.Status;

    public string Message { get; private set; } = endpointEvent.Message ?? string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string? GetDataProperty(string pointer)
    {
        if (Data == null)
        {
            return null;
        }

        try
        {
            var jsonData = JsonNode.Parse(Data);
            var jsonPointer = JsonPointer.Parse(pointer);
            return jsonPointer.TryEvaluate(jsonData, out var result) ? result?.GetValue<string>() : null;
        }
        catch
        {
            return null;
        }
    }

    public void UpdateStatus(EventStatus status, string? message)
    {
        Status = status;
        NotifyPropertyChanged(nameof(Status));

        Message = message ?? string.Empty;
        NotifyPropertyChanged(nameof(Message));
    }

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new(propertyName));
    }
}