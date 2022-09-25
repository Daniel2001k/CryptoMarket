using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMarket.Hub.Server.Controllers;

[ApiController]
public class ErrorHandlingController : ControllerBase
{
    private readonly ILogger<ErrorHandlingController> _logger;

    public ErrorHandlingController(ILogger<ErrorHandlingController> logger)
    {
        _logger = logger;
    }

    [Route("/error")]
    public IActionResult HandleError([FromServices] IHostEnvironment hostEnvironment)
    {
        var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
        var stackTrace = hostEnvironment.IsDevelopment() ? exceptionHandlerFeature.Error.StackTrace : null;

        _logger.LogError(exceptionHandlerFeature.Error, "Unhandled exception occurred");

        return Problem(detail: stackTrace, title: exceptionHandlerFeature.Error.Message);
    }
}

