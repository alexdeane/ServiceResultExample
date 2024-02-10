using ServiceResultExample.Client;
using ServiceResultExample.Common;

namespace ServiceResultExample.Example3;

/// <summary>
/// The third service uses a much better result wrapper
/// </summary>
public interface IService3
{
    public Task<ServiceResult<WeatherForecast, ServiceError>> Handle();
}

public class Service3 : IService3
{
    private readonly ILogger<Service3> _logger;
    private readonly IClient _client;

    public Service3(ILogger<Service3> logger, IClient client)
    {
        _logger = logger;
        _client = client;
    }

    // The signature will be slightly longer here
    public async Task<ServiceResult<WeatherForecast, ServiceError>> Handle()
    {
        try
        {
            // The implicit casts on ServiceResult<T1, T2> allow us to directly return
            // our result object
            return await _client.GetForecast();
        }
        // If we're invoking more than one dependency, we can keep track of which things failed
        // using custom exception types
        catch (CustomClientException clientException)
        {
            _logger.LogError(clientException, "Exception occured in client");

            // Same here! No need for extra fluff, you can directly return
            // your error object!
            return new ServiceError(
                ErrorMessage: "Exception occured in client",
                ErrorType.Dependency
            );
        }
    }

    public async Task<ServiceResult<WeatherForecast, ServiceError>> HandlePartial()
    {
        // For this scenario, pretend it's possible to return a partial success (error AND data some data).
        // Perhaps we're streaming data from our data store and we hit a transient connection error, or perhaps
        // we are aggregating results from multiple clients and only a subset of them fail.
        
        // Here we can directly return a tuple containing our success AND our error
        return (
            new WeatherForecast(),
            new ServiceError(ErrorMessage: "error", ErrorType.Server)
        );
    }
}