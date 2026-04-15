using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Query;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Builders.CalendarOfItems;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Models;

namespace SchoolAccount.Web.Connect.Controllers;

[Authorize]
public class CalendarController(
    IQueryHandler<CalendarOfItemsDirectionalQuery, CalendarOfItemsPagedResult> handler,
    IOrganisationContext organisationContext
) : Controller
{
    [HttpGet(RouteConstants.Calendar.Index)]
    public async Task<IActionResult> Index(
        [FromQuery] CalendarQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var filter = new GetSubTasksByDirectionForTabViewCalendarOfItemsQuery(
            query.ViewModes,
            query.PageSize,
            query.PageNumber,
            query.Filters,
            query.SortMode
        );
        var result = await handler.Handle(
            filter, 
            cancellationToken);

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
