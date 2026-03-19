using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Query;
using SchoolAccount.Kernel.CalendarOfItems;

namespace SchoolAccount.Web.Connect.Controllers;

[Authorize]
public class CalendarController(IQueryHandler<CalendarOfItemsDirectionalQuery, CalendarOfItemsPagedResult> queryHandler)
    : Controller
{
    [HttpGet(RouteConstants.Calendar.Index)]
    public async Task<IActionResult> Index(
        [FromQuery] CalendarOfItemsDirectionalQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var queryModel = query with
        {
            ToQuery = CalendarOfItemsQueryTypes.SubTask,
            ViewMode =
                query.ViewMode == CalendarOfItemsViewMode.NotSpecified
                    ? CalendarOfItemsViewMode.Forward
                    : query.ViewMode,
            ViewPeriodInMonths = 12,
            QueryFromDate = DateOnlyExtensions.Today,
            PageSize = query.PageSize <= 0 ? 10 : query.PageSize,
            PageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber,
        };
        var result = await queryHandler.Handle(queryModel, cancellationToken);
        return Json(
            new
            {
                result.IsSuccess,
                Payload = result.IsSuccess ? result.Value : null,
                Error = result.IsFailure ? result.Error : null,
            }
        );
    }
}
