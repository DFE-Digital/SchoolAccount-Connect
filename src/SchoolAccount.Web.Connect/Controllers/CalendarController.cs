using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.CalendarOfItems.Query;
using SchoolAccount.Application.Features.CalendarOfItems.Query.GetCalendarOfItemsOfSubTasksByDirectionForTabView;
using SchoolAccount.Application.Features.CalendarOfItems.Query.Operational;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Builders.CalendarOfItems;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Models;
using static SchoolAccount.Web.Connect.RouteConstants;

namespace SchoolAccount.Web.Connect.Controllers;

public class CalendarController(
    IQueryHandler<CalendarOfItemsDirectionalQuery, QueryPagedResult<CalendarOfItemsRow>> handler,
    IOrganisationContext organisationContext
) : Controller
{
    [HttpGet(Calendar.Index)]
    public async Task<IActionResult> Index(
        [FromQuery] CalendarQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var filter = new GetCalendarOfItemsOfSubTasksByDirectionForTabViewQuery(
            query.ViewModes,
            query.PageSize,
            query.PageNumber,
            query.Filters,
            query.SortMode
        );
        var result = await handler.Handle(filter, cancellationToken);

        if (result.IsFailure)
        {
            throw new ApplicationException(result.Error.Description);
        }

        var currentUri = Request.GetFullRequestUri();
        var viewBuilder = new CalendarOfItemsViewBuilder(organisationContext);
        var viewModel = viewBuilder.BuildForPage(result.Value, filter.ViewModes, currentUri);

        return View(viewModel);
    }
}
