using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Web.Connect.Attributes;
using static SchoolAccount.Web.Connect.RouteConstants;

namespace SchoolAccount.Web.Connect.Features.Cookies;

public sealed class CookiesController : Controller
{
    [Breadcrumb("Home", Root)]
    [Breadcrumb("Cookies", RouteConstants.Cookies)]
    [HttpGet(RouteConstants.Cookies)]
    [AllowAnonymous]
    public IActionResult Cookies()
    {
        return View(ViewAddressConstants.Cookies);
    }
}
