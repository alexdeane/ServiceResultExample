using Microsoft.AspNetCore.Mvc;
using ServiceResultExample.Clients;
using ServiceResultExample.Services;
using ServiceResultExample.Services.Service1;
using ServiceResultExample.Services.Service2;
using ServiceResultExample.Services.Service3;

namespace ServiceResultExample.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IService1 _service1;
    private readonly IService2 _service2;
    private readonly IService3 _service3;

    public WeatherForecastController(IService1 service1, IService2 service2, IService3 service3)
    {
        _service1 = service1;
        _service2 = service2;
        _service3 = service3;
    }

    /// <summary>
    /// The naive solution. Expect the primary service layer to propagate exceptions,
    /// and catch them in the controller. The only thing we can sort of safely return
    /// to the user is the exception message, and even that will require us to trust
    /// that whatever threw the exception did so with a user-safe message.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get1()
    {
        try
        {
            var weatherForecast = await _service1.Handle();
            return Ok(weatherForecast);
        }
        // You would catch other exception types here for different status codes.
        // For example, in a database scenario you may want to return a 4xx error if
        // the user attempts to update a record that does not exist.
        //
        // This could be replaced by an exception middleware,
        // but fundamentally it is the same thing.
        catch (CustomClientException clientException)
        {
            var error = new ApiError(clientException.Message);

            return new ObjectResult(error)
            {
                StatusCode = StatusCodes.Status502BadGateway
            };
        }
        catch(Exception exception)
        {
            // You need to trust that the exception message is user-safe
            var error = new ApiError(exception.Message);

            return new ObjectResult(error)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }

    /// <summary>
    /// The intermediate solution. Our service returns a result wrapper
    /// which may contain error information.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get2()
    {
        var serviceResult = await _service2.Handle();

        // We could either return this object directly,
        // or as shown here use the existence of the error
        // to determine status code.
        if (serviceResult.Error is null)
        {
            return Ok(serviceResult.Result);
        }

        var serviceError = serviceResult.Error.Value;

        // Result object is easy to create. Our error message is already predefined by the lower-level
        // code. We can be certain it is user-safe because it is not directly coming from an exception.
        //
        // Status code determination is a responsibility of the controller, which decouples protocl-specific
        // error information from your application core.
        return BuildError(serviceError);
    }

    /// <summary>
    /// The intermediate solution. Our service returns a result wrapper
    /// which may contain error information.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get3()
    {
        var serviceResult = await _service3.Handle();

        // The Switch method presents a very streamlined way
        // of returning different objects of the same type
        // (in this case IActionResults) based on the success
        // of the service, while maintaining full type-safety
        return serviceResult.Switch(
            successHandler: Ok,
            errorHandler: BuildError,
            partialSuccessHandler: (forecast, error) =>
            {
                // Do something here for a partial success,
                // or optionally remove this handler if we don't
                // expect partial success to be a valid scenario
                return Ok();
            }
        );
    }

    private static IActionResult BuildError(ServiceError error)
    {
        return new ObjectResult(value: new ApiError(error.ErrorMessage))
        {
            StatusCode = error.ErrorType switch
            {
                ErrorType.Business => StatusCodes.Status400BadRequest,
                ErrorType.Dependency => StatusCodes.Status502BadGateway,
                ErrorType.Server => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            }
        };
    }
}