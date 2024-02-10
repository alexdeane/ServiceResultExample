using ServiceResultExample.Client;
using ServiceResultExample.Common;

namespace ServiceResultExample.Example2_Intermediate;

/// <summary>
/// A better solution, but still not quite there. This
/// service uses the <see cref="ServiceResult{T}"/> class
/// as a wrapper for its result object
/// </summary>
public interface IService2
{
    public Task<ServiceResult<WeatherForecast>> Handle();
}

public class Service2 : IService2
{
    private readonly ILogger<Service2> _logger;
    private readonly IClient _client;

    public Service2(ILogger<Service2> logger, IClient client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task<ServiceResult<WeatherForecast>> Handle()
    {
        try
        {
            var result = await _client.GetForecast();

            // Not bad - not too much extra code for the result return
            return ServiceResult.ForResult(result);
        }
        // If we're invoking more than one dependency, we can keep track of which things failed
        // using custom exception types
        catch (CustomClientException clientException)
        {
            _logger.LogError(clientException, "Exception occured in client");

            var error = new ServiceError(
                ErrorMessage: "Exception occured in client",
                ErrorType.Dependency
            );

            // There is no way of getting around having to specify a
            // type parameter for generating an error result 
            return ServiceResult.ForError<WeatherForecast>(error);
        }
    }
}