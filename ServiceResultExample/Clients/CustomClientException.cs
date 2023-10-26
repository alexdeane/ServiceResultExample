namespace ServiceResultExample.Clients;

/// <summary>
/// This custom exception serves to wrap exceptions of other types
/// which are thrown in the client.
///
/// Doing this allows the primary service layer to use one big try/catch block
/// rather than a bunch of little ones when it is responsible for invoking multiple
/// clients, repositories, etc. 
/// </summary>
public class CustomClientException : ApplicationException
{
    public CustomClientException()
    {
    }

    public CustomClientException(string message) : base(message)
    {
    }

    public CustomClientException(string message, Exception innerException) : base(message, innerException)
    {
    }
}