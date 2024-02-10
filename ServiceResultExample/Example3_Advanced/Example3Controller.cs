using Microsoft.AspNetCore.Mvc;
using ServiceResultExample.Common;

namespace ServiceResultExample._Advanced;

[ApiController]
[Route("api/[action]")]
public class Example3Controller : ControllerBase
{
    private readonly IService3 _service3;

    public Example3Controller(IService3 service3)
    {
        _service3 = service3;
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