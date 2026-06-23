using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SchoolAccount.Web.Connect.Attributes;
using SchoolAccount.Web.Connect.Settings;
using static SchoolAccount.Web.Connect.RouteConstants;

namespace SchoolAccount.Web.Connect.Features.About;

public sealed class AboutController(IHostEnvironment env, IOptions<CustomEnvironmentSettings> customEnvSettings)
    : Controller
{
    [Breadcrumb("Home", Root)]
    [HttpGet(RouteConstants.About)]
    [AllowAnonymous]
    public IActionResult About()
    {
        var model = new AboutViewModel
        {
            Version = Assembly
                .GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion,
            Environment = $"{customEnvSettings.Value.Label}  {env.EnvironmentName}".Trim(),
            DeploymentDate = GetDeploymentFileSystemDate(),
        };
        return View(ViewAddressConstants.About, model);
    }

    [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase")]
    private string GetDeploymentFileSystemDate()
    {
        var filePath = Path.Combine(env.ContentRootPath, "appsettings.json");

        if (System.IO.File.Exists(filePath))
        {
            DateTime fileTime = System.IO.File.GetLastWriteTimeUtc(filePath);
            return fileTime.ToString("d MMMM yyyy 'at' h:mmtt", CultureInfo.InvariantCulture);
        }

        return "Unknown";
    }
}
