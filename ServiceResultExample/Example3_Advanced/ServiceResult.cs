namespace ServiceResultExample._Advanced;

/// <summary>
/// Abstraction used to return a result, error, or both
/// </summary>
public record ServiceResult<TResult, TError>(TResult? Result = null, TError? Error = null)
    where TResult : class where TError : class
{
    /// <summary>
    /// Implicit cast to allow returning an instance of <see cref="TError"/>
    /// from a method
    /// </summary>
    public static implicit operator ServiceResult<TResult, TError>(TError error)
        => new(Error: error);

    /// <summary>
    /// Implicit cast to allow returning an instance of <see cref="TResult"/>
    /// from a method
    /// </summary>
    public static implicit operator ServiceResult<TResult, TError>(TResult result)
        => new(Result: result);

    /// <summary>
    /// Implicit cast to allow returning a Tuple of the form (<see cref="TResult"/>, <see cref="TError"/>).
    /// This is to support partial success scenarios
    /// </summary>
    public static implicit operator ServiceResult<TResult, TError>(ValueTuple<TResult, TError> tuple)
        => new(Result: tuple.Item1, Error: tuple.Item2);

    /// <summary>
    /// Convenience method allowing one to switch on the result and supply
    /// handlers for different cases. This override allows handlers to not return a value.
    /// </summary>
    /// <param name="successHandler">A handler to be invoked when the result is a success</param>
    /// <param name="errorHandler">A handler to be invoked when the result is an error</param>
    public void Switch(Action<TResult> successHandler, Action<TError> errorHandler)
    {
        if (Result is not null)
            successHandler(Result);
        else
            errorHandler(Error!);
    }

    /// <summary>
    /// Convenience method allowing one to switch on the result and supply
    /// handlers for different cases and return values. This override allows handlers
    /// to return a value.
    /// </summary>
    /// <param name="successHandler">A handler to be invoked when the result is a success</param>
    /// <param name="errorHandler">A handler to be invoked when the result is an error</param>
    public T Switch<T>(Func<TResult, T> successHandler, Func<TError, T> errorHandler)
    {
        if (Result is not null)
            return successHandler(Result);

        return errorHandler(Error!);
    }

    /// <summary>
    /// Convenience method allowing one to switch on the result and supply
    /// handlers for different cases and return values, including a partial success.
    /// This override allows the handlers to return a value.
    /// This override allows the user to specify a handler for partial success.
    /// </summary>
    /// <param name="successHandler">A handler to be invoked when the result is a success</param>
    /// <param name="errorHandler">A handler to be invoked when the result is an error</param>
    /// <param name="partialSuccessHandler">A handler to be invoked when the result is a success <b>and</b> an error</param>
    public void Switch(Action<TResult> successHandler, Action<TError> errorHandler,
        Action<TResult, TError> partialSuccessHandler)
    {
        if (Result is not null && Error is not null)
            partialSuccessHandler(Result, Error);
        else if (Result is not null)
            successHandler(Result);
        else errorHandler(Error!);
    }

    /// <summary>
    /// Convenience method allowing one to switch on the result and supply
    /// handlers for different cases and return values, including a partial success.
    /// This override allows the handlers to return a value.
    /// This override allows the user to specify a handler for partial success.
    /// </summary>
    /// <param name="successHandler">A handler to be invoked when the result is a success</param>
    /// <param name="errorHandler">A handler to be invoked when the result is an error</param>
    /// <param name="partialSuccessHandler">A handler to be invoked when the result is a success <b>and</b> an error</param>
    public T Switch<T>(Func<TResult, T> successHandler, Func<TError, T> errorHandler,
        Func<TResult, TError, T> partialSuccessHandler)
    {
        if (Result is not null && Error is not null)
            return partialSuccessHandler(Result, Error);

        if (Result is not null)
            return successHandler(Result);

        return errorHandler(Error!);
    }
}