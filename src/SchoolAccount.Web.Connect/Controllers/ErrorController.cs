using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Integration.DfESignIn.Exceptions;
using SchoolAccount.Web.Connect.Models;

namespace SchoolAccount.Web.Connect.Controllers;

public class ErrorController(
    ILogger<ErrorController> logger, 
    IWebHostEnvironment environment
) : Controller
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
            OriginalPath = reExecute?.OriginalPath ?? exceptionFeature?.Path ?? HttpContext.Request.Path
        };

        var simplified = code.HasValue &&
                         (code >= StatusCodes.Status400BadRequest && code < StatusCodes.Status500InternalServerError)
            ? StatusCodes.Status400BadRequest
            : code ?? StatusCodes.Status500InternalServerError;

        switch (exceptionFeature?.Error)
        {
            case ProviderAuthorisationException:
                model.HandledException = true;
                model.Title = "Service Inaccessible";
                model.Heading = "Sorry, you cannot currently use this service";
                model.Messages =
                [
                    "You cannot use this service as it&#x2019;s currently only available to pre-16 academies.",
                    "We're working to expand this service to all academies and maintained schools in future.",
                    "<div class=\"govuk-!-margin-bottom-9\"></div>",
                    "<a href=\"/login/logout\" class=\"govuk-link\">Sign out and return to DfE Connect start page</a>"
                ];
                break;
            default:
                switch (simplified)
                {
                    case StatusCodes.Status400BadRequest:
                        model.Title = "Not Found";
                        model.Heading = "Page not found";
                        model.Messages =
                        [
                            "If you typed the web address, check it is correct.",
                            "If you pasted the web address, check you copied the entire address.",
                            "If the web address is correct or you selected a link or button, <a href=\"#\" class=\"govuk-link\">contact the DfE Helpline</a> if you need any further assistance."
                        ];
                        break;
                    default:
                        model.Title = "Problem with the Service";
                        model.Heading = "Sorry, there is a problem with the service";
                        model.Messages =
                        [
                            "Try again later.",
                            "You can try go <a class=\"govuk-link\" href=\"/\">back to your dashboard</a>.",
                            "<a class=\"govuk-link\" href=\"#\">Contact the DfE Helpline</a> if you need to speak to someone for any further assistance."
                        ];
                        break;
                }

                break;
        }

#pragma warning disable CA1508
#pragma warning disable CA1727
        if (model.Exception is not null)
        {
            logger.LogCritical(model.Exception, "{route} : An exception occured. {message}", model.OriginalPath, model.Exception?.Message);
        }
        else
        {
            logger.LogWarning("{route} : An issue occured.", model.OriginalPath);
        }
#pragma warning restore CA1508
#pragma warning restore CA1727
        
        return View(model);
    }
}
