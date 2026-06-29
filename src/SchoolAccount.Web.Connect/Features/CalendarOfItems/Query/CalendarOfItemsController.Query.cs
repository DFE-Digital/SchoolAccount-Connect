using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Constants;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.GetCalendarOfItemsOfSubTasksByDirectionForTabView;
using SchoolAccount.Web.Connect.Builders.CalendarOfItems;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Models;

// ReSharper disable CheckNamespace

namespace SchoolAccount.Web.Connect.Features.CalendarOfItems;

public sealed partial class CalendarOfItemsController
{
    [HttpGet(CalendarOfItemsConstants.Routes.Query)]
    public async Task<IActionResult> Query(
        [FromQuery] CalendarQuery query,
        CancellationToken cancellationToken = default
    )
    {
        var viewModes = query.ViewModes;

        if (viewModes == CalendarOfItemsViewModes.None)
        {
            viewModes |= CalendarOfItemsViewModes.Forward;
        }

        if (await featureManager.IsEnabledAsync(FeatureFlagConstants.CalendarOfItemsViewModesKanban))
        {
            viewModes |= CalendarOfItemsViewModes.Kanban;
        }

        var filter = new GetCalendarOfItemsOfSubTasksByDirectionForTabViewQuery(
            viewModes,
            12,
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

        return View(CalendarOfItemsConstants.Views.Query, viewModel);
    }
}
