using Microsoft.AspNetCore.Mvc;
using ServiceResultExample.Common;

namespace ServiceResultExample.Example2;

[ApiController]
[Route("api/[action]")]
public class Example2Controller : ControllerBase
{
    private readonly IService2 _service2;

    public Example2Controller(IService2 service2)
    {
        _service2 = service2;
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

        // Result object is easy to create. Our error message is already predefined by the lower-level
        // code. We can be certain it is user-safe because it is not directly coming from an exception.
        //
        // Status code determination is a responsibility of the controller, which decouples protocl-specific
        // error information from your application core.
        return BuildError(serviceResult.Error);
    }

    private static IActionResult BuildError(ServiceError error) =>
        new ObjectResult(value: new ApiError(error.ErrorMessage))
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