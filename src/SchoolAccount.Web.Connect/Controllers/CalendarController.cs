using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.CalendarOfItems.Query;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Web.Connect.Builders.Interfaces;
using SchoolAccount.Web.Connect.Models;

namespace SchoolAccount.Web.Connect.Controllers;

[Authorize]
public class CalendarController(ICalendarOfItemsViewBuilder viewBuilder) : Controller
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
                query.Filters.Select(filter => new FilterRequest
                {
                    Field = filter.Key,
                    Operator = filter.Key switch
                    {
                        "name" => ComparisonType.Contains,
                        _ => ComparisonType.In,
                    },
                    Value = filter.Key switch
                    {
                        "name" => filter.Value,
                        _ => filter.Value.GetType() == typeof(string)
                            ? filter.Value.ToString()?.Split(',').ToList()
                            : filter.Value,
                    },
                })
            )
        );

        return View(await viewBuilder.BuildForPage(queryModel, cancellationToken));
    }
}
