using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Dashboard;
using SchoolAccount.Kernel;
using static SchoolAccount.Web.Connect.RouteConstants;

namespace SchoolAccount.Web.Connect.Features.Dashboard;

public sealed class DashboardController(
    IQueryHandler<GetDashboardQuery, GetDashboardResponse> dashboardHandler,
    IDateTimeProvider dateTimeProvider
) : Controller
{
    [HttpGet(Root)]
    public async Task<IActionResult> Dashboard(CancellationToken cancellationToken)
    {
        var query = new GetDashboardQuery(dateTimeProvider.Today);
        var dashboardResult = await dashboardHandler.Handle(query, cancellationToken);

        if (dashboardResult.IsFailure)
        {
            return Problem(detail: dashboardResult.Error.Description);
        }

        var viewModel = DashboardViewModelBuilder.Build(dashboardResult.Value);

        return View(ViewAddressConstants.Dashboard, viewModel);
    }
}
