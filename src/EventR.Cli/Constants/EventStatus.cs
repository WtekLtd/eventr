namespace EventR.Cli.Constants;

public enum EventStatus
{
    /// <summary>
    /// Sent to the endpoint for processing.
    /// </summary>
    Sent,

    /// <summary>
    /// Received by the endpoint.
    /// </summary>
    Received,

    /// <summary>
    /// Successfully processed by the endpoint.
    /// </summary>
    ProcessingSuccessful,

    /// <summary>
    /// Unsuccessfully processed by the endpoint.
    /// </summary>
    ProcessingFailed,

    /// <summary>
    /// Published by the endpoint.
    /// </summary>
    Published,

    LoggedTrace,

    LoggedDebug,

    LoggedInformation,

    LoggedWarning,

    LoggedError,

    LoggedCritical
}
