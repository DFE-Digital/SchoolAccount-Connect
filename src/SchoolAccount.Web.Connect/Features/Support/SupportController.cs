using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Web.Connect.Attributes;
using static SchoolAccount.Web.Connect.RouteConstants;

namespace SchoolAccount.Web.Connect.Features.Support;

public sealed class SupportController : Controller
{
    [Breadcrumb("Home", Root)]
    [Breadcrumb("Support", RouteConstants.Support)]
    [HttpGet(RouteConstants.Support)]
    public IActionResult Support()
    {
        return View(ViewAddressConstants.Support);
    }
}
