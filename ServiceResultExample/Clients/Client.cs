namespace ServiceResultExample.Clients;

/// <summary>
/// Fake client that returns random data. Let's pretend that this
/// calls an external API. It allows any thrown exceptions to propagate
/// upwards. 
/// </summary>
public interface IClient
{
    Task<WeatherForecast> GetForecast();
}

public class Client : IClient
{
    private static readonly string[] Summaries = 
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public async Task<WeatherForecast> GetForecast()
    {
        try
        {
            await Task.Delay(1000);

            // Simulate a transient network error
            if (Random.Shared.Next(0, 2) == 0)
            {
                throw new HttpRequestException("Failed to connect to the weather API");
            }

            return new WeatherForecast
            {
                Date = DateTime.Now.AddDays(Random.Shared.Next(0, 24)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            };
        }
        catch (Exception exception) when (exception is not CustomClientException)
        {
            throw new CustomClientException("Exception occurred in client", exception);
        } 
    }
}