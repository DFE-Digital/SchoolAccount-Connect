using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Integration.DfESignIn.Exceptions;
using SchoolAccount.Web.Connect.Features;

namespace SchoolAccount.Web.Connect.Features.Error;

[AllowAnonymous]
public partial class ErrorController(ILogger<ErrorController> logger, IWebHostEnvironment environment) : Controller
{
    [Route(RouteConstants.Error)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int? code)
    {
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        var reExecute = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

        var model = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            ShowException = !environment.IsProduction(),
            Exception = exceptionFeature?.Error,
            OriginalPath = reExecute?.OriginalPath ?? exceptionFeature?.Path ?? HttpContext.Request.Path,
            SignOutUrl = Url.Action("SignOut", "Account", new { area = "MicrosoftIdentity" }),
        };

        switch (exceptionFeature?.Error)
        {
            case ProviderAuthorisationException:
                LogWarning(model.OriginalPath);
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return View("InaccessibleProvider", model);
            case not null:
                LogCriticalError(model.OriginalPath, model.Exception!.Message, model.Exception);
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return View("ServiceFailure", model);
        }

        LogWarning(model.OriginalPath);

        if (code is >= StatusCodes.Status400BadRequest and < StatusCodes.Status500InternalServerError)
        {
            Response.StatusCode = StatusCodes.Status404NotFound;
            return View("NotFound", model);
        }

        Response.StatusCode = StatusCodes.Status500InternalServerError;
        return View("ServiceFailure", model);
    }

    [LoggerMessage(EventId = 1001, Level = LogLevel.Critical, Message = "{route} : An exception occured. {message}")]
    private partial void LogCriticalError(string route, string message, Exception exception);

    [LoggerMessage(EventId = 1002, Level = LogLevel.Warning, Message = "{route} : An issue occured.")]
    private partial void LogWarning(string route);
}
