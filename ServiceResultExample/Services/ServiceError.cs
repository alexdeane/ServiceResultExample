namespace ServiceResultExample.Services;

/// <summary>
/// Common type for an error returned by a core service
/// </summary>
/// <param name="ErrorMessage"></param>
/// <param name="ErrorType"></param>
public readonly record struct ServiceError(string ErrorMessage, ErrorType ErrorType);

/// <summary>
/// This enum simply indicates to the controller
/// or web interface <b>who's fault</b> the error was,
/// and does not make a determination on status code.
///
/// These values can be customized however you want for your specific use-case
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// Business logic caused the request to fail
    /// </summary>
    Business,

    /// <summary>
    /// A dependency failed and the request is irrecoverable
    /// </summary>
    Dependency,

    /// <summary>
    /// An unexpected error occurred on the server side
    /// </summary>
    Server
}