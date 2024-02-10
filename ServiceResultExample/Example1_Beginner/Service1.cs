using ServiceResultExample.Client;

namespace ServiceResultExample.Example1;

/// <summary>
/// This service simply allows exceptions to propagate
/// </summary>
public interface IService1
{
    public Task<WeatherForecast> Handle();
}

public class Service1 : IService1
{
    private readonly IClient _client;

    public Service1(IClient client)
    {
        _client = client;
    }

    // In the very simply case shown here,
    // you would be correct to say that this service can be deleted.
    // However, in a real world scenario the service may be responsible
    // for invoking multiple clients, databases, etc.
    public Task<WeatherForecast> Handle()
    {
        return _client.GetForecast();
    }
}