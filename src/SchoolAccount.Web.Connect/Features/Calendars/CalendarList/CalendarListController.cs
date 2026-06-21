using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Query;
using SchoolAccount.Application.Features.CalendarOfItems.Query.Operational;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Extensions;

namespace SchoolAccount.Web.Connect.Features.Calendars.CalendarList;

public class CalendarListController(
    IQueryHandler<CalendarOfItemsDirectionalQuery, CalendarOfItemsPagedResult> handler,
    IOrganisationContext organisationContext
) : Controller
{
    [HttpGet(RouteConstants.Calendar.Index)]
    public async Task<IActionResult> Index(
        [FromQuery] CalendarListRequest request,
        CancellationToken cancellationToken = default
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var filter = new GetSubTasksByDirectionForTabViewCalendarOfItemsQuery(
            request.ViewModes,
            request.PageSize,
            request.PageNumber,
            request.Filters,
            request.SortMode
        );
        var result = await handler.Handle(filter, cancellationToken);

        if (result.IsFailure)
        {
            throw new ApplicationException(result.Error.Description);
        }

        var currentUri = Request.GetFullRequestUri();
        var viewBuilder = new CalendarListViewModelBuilder(organisationContext);
        var viewModel = viewBuilder.BuildForPage(result.Value, filter.ViewModes, currentUri);

        return View(ViewAddressConstants.CalendarOfItems.Index, viewModel);
    }
}
