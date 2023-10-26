namespace ServiceResultExample.Services.Service2;

public class ServiceResult<T>
{
    public T? Result { get; init; }
    public ServiceError? Error  { get; init; }
}

public static class ServiceResult
{
    public static ServiceResult<T> ForResult<T>(T result)
        => new()
        {
            Result = result
        };

    public static ServiceResult<T> ForError<T>(ServiceError error)
        => new()
        {
            Error = error
        };
}