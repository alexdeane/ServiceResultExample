namespace ServiceResultExample.Common;

/// <summary>
/// Error model returned by the API when something goes wrong
/// </summary>
/// <param name="ErrorMessage"></param>
public record ApiError(string ErrorMessage);