namespace ServiceResultExample.Services.Service2;

/// <summary>
/// Small, naive result wrapper
/// </summary>
public record ServiceResult<T>
{
    /// <summary>
    /// The result, of there is one
    /// </summary>
    public T? Result { get; init; }

    /// <summary>
    /// An error, if one occurred
    /// </summary>
    public ServiceError? Error { get; init; }
}

/// <summary>
/// Static helper class for some syntactic sugar.
/// This is optional for this result mechanism. 
/// </summary>
public static class ServiceResult
{
    /// <summary>
    /// Convenience method to supply a slightly neater way
    /// to initialize a successful result.
    /// </summary>
    public static ServiceResult<T> ForResult<T>(T result)
        => new()
        {
            Result = result
        };

    /// <summary>
    /// For consistency, a parallel method for producing
    /// an errored result.
    /// </summary>
    public static ServiceResult<T> ForError<T>(ServiceError error)
        => new()
        {
            Error = error
        };

    /// <summary>
    /// Final convenience method for a partial success
    /// </summary>
    public static ServiceResult<T> ForPartialSuccess<T>(T result, ServiceError error)
        => new()
        {
            Result = result,
            Error = error
        };
}