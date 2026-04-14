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
        var queryModel = new CalendarOfItemsDirectionalQuery(
            CalendarOfItemsQueryTypes.SubTask,
            query.ViewModes == CalendarOfItemsViewModes.None ? CalendarOfItemsViewModes.Forward : query.ViewModes,
            12,
            DateOnlyExtensions.Today,
            query.PageSize <= 0 ? 10 : query.PageSize,
            query.PageNumber <= 0 ? 1 : query.PageNumber,
            query.SortMode,
            new CalendarOfItemsFilter(
                query.Filters
                    .Select(filter => new FilterRequest
                    {
                        Field = filter.Key,
                        Operator = filter.Key switch
                        {
                            "name" => ComparisonType.Contains,
                            _ => ComparisonType.In
                        },
                        Value = filter.Key switch
                        {
                            "name" => filter.Value,
                            _ => filter.Value.GetType() == typeof(string)
                                ? filter.Value.ToString()?.Split(',').ToList()
                                : filter.Value
                        }
                    }))
        );

        var result = await handler.Handle(queryModel, cancellationToken);

        if (result.IsFailure)
        {
            throw new ApplicationException(result.Error.Description);
        }

        var currentUri = Request.GetFullRequestUri();
        var viewBuilder = new CalendarOfItemsViewBuilder(organisationContext);
        var viewModel = viewBuilder.BuildForPage(result.Value, queryModel.ViewModes, currentUri);

        return View(viewModel);
    }
}
