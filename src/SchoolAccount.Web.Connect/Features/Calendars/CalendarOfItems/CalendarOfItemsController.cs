using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Query;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Extensions;

namespace SchoolAccount.Web.Connect.Features.Calendars.CalendarOfItems;

public class CalendarOfItemsController(IMediator mediator, IOrganisationContext organisationContext) : Controller
{
    [HttpGet(RouteConstants.Calendar.CalendarOfItems)]
    public async Task<IActionResult> GetCalendarOfItems(
        [FromQuery] CalendarOfItemsRequest request,
        CancellationToken cancellationToken = default
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var filter = new CalendarOfItemsQuery(
            request.ViewModes,
            request.PageSize,
            request.PageNumber,
            request.Filters,
            request.SortMode
        );

        var result = await mediator.Query(filter, cancellationToken);

        if (result.IsFailure)
        {
            throw new ApplicationException(result.Error.Description);
        }

        var currentUri = Request.GetFullRequestUri();
        var viewBuilder = new CalendarOfItemsViewModelBuilder(organisationContext);
        var viewModel = viewBuilder.BuildForPage(result.Value, filter.ViewModes, currentUri);

        return View("CalendarOfItems", viewModel);
    }
}
