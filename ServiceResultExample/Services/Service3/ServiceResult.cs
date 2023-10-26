namespace ServiceResultExample.Services.Service3;

/// <summary>
/// Abstraction used to return a result, error,
/// or both
/// </summary>
/// <param name="Result">The result object</param>
/// <param name="Error">The error object</param>
/// <typeparam name="TResult">Result type</typeparam>
/// <typeparam name="TError">Error type</typeparam>
public record ServiceResult<TResult, TError>(TResult? Result = default, TError? Error = default)
{
    /// <summary>
    /// Implicit cast to allow returning an instance of <see cref="TError"/>
    /// from a method
    /// </summary>
    /// <param name="error"></param>
    /// <returns>
    /// A <see cref="ServiceResult{TResult,TError}"/> with only
    /// the <see cref="ServiceResult{TResult,TError}.Error"/> property set
    /// </returns>
    public static implicit operator ServiceResult<TResult, TError>(TError error)
        => new(Error: error);

    /// <summary>
    /// Implicit cast to allow returning an instance of <see cref="TResult"/>
    /// from a method
    /// </summary>
    /// <param name="result"></param>
    /// <returns>
    /// A <see cref="ServiceResult{TResult,TError}"/> with only
    /// the <see cref="ServiceResult{TResult,TError}.Result"/> property set
    /// </returns>
    public static implicit operator ServiceResult<TResult, TError>(TResult result)
        => new(Result: result);

    /// <summary>
    /// Implicit cast to allow returning a Tuple of the form (<see cref="TResult"/>, <see cref="TError"/>).
    /// This is to support partial success scenarios
    /// </summary>
    /// <param name="tuple"></param>
    /// <returns>
    /// A <see cref="ServiceResult{TResult,TError}"/> with both properties set
    /// </returns>
    public static implicit operator ServiceResult<TResult, TError>(ValueTuple<TResult, TError> tuple)
        => new(Result: tuple.Item1, Error: tuple.Item2);

    public T Switch<T>(Func<TResult, T> successHandler, Func<TError, T> errorHandler)
    {
        if (Result is not null)
            return successHandler(Result);

        return errorHandler(Error!);
    }

    public T Switch<T>(Func<TResult, T> successHandler, Func<TError, T> errorHandler, Func<TResult, TError, T> partialSuccessHandler)
    {
        if (Result is not null && Error is not null)
            return partialSuccessHandler(Result, Error);

        if (Result is not null)
            return successHandler(Result);

        return errorHandler(Error!);
    }
}