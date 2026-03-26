using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Http.Extensions;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Query;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Builders.Interfaces;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Models.CalendarOfItems;

namespace SchoolAccount.Web.Connect.Builders;

public class CalendarOfItemsViewBuilder(
    IQueryHandler<CalendarOfItemsDirectionalQuery, CalendarOfItemsPagedResult> directionalQueryBuilder,
    IQueryHandler<CalendarOfItemsCustomQuery, CalendarOfItemsPagedResult> customQueryBuilder,
    ICalendarOfItemsRowViewBuilder rowViewBuilder,
    IPaginationViewBuilder paginationBuilder,
    IOrganisationContext organisationContext,
    IHostEnvironment environment,
    IHttpContextAccessor contextAccessor
) : ICalendarOfItemsViewBuilder
{
    public CalendarOfItemsViewModel Build(CalendarOfItemViewOptions options, CalendarOfItemsPagedResult result)
    {
        Collection<CalendarOfItemsRowGroupViewModel> rows = [];

        if (result.Payload.Count > 0)
        {
            rows = result
                .Payload.GroupBy(x => options.GroupingFunction is not null ? options.GroupingFunction(x) : string.Empty)
                .Select(x => new CalendarOfItemsRowGroupViewModel(
                    x.Key,
                    x.Select(r => rowViewBuilder.Build(options.ViewMode, r))
                ))
                .ToCollection();
        }

        return new CalendarOfItemsViewModel(
            options.Title,
            options.Description,
            options.ViewMode,
            options.Tabs ?? [],
            rows,
            paginationBuilder.Build(result)
        )
        {
            GeneratedAt = result.GeneratedDate,
            LastUpdatedMessage = options.LastUpdatedMessage,
            CallToActionMessage = options.CallToActionMessage,
            NoResultsMessage = options.NoResultsMessage,
            Heading = options.Heading,
            SubHeading = options.SubHeading,
            Title = options.Title,
            Description = options.Description,
            Caption = organisationContext.Organisation.Name,
        };
    }

    public async Task<CalendarOfItemsViewModel> BuildForPage(
        CalendarOfItemsDirectionalQuery query,
        CancellationToken cancellationToken
    )
    {
        var result = await directionalQueryBuilder.Handle(query, cancellationToken);

        if (result.IsFailure)
        {
            throw new ApplicationException(result.Error.Description);
        }

        var url = contextAccessor.HttpContext!.Request.GetDisplayUrl();

        CalendarOfItemsTabViewModel BuildTab(CalendarOfItemsViewModes mode, string label, string? description = null)
        {
            var updatedUrl = UriExtensions.AddOrUpdateQuery(
                url,
                environment,
                (nameof(CalendarOfItemsDirectionalQuery.ViewModes), mode)
            );
            var correctedUrl = UriExtensions.RemoveByKeyQuery(updatedUrl, environment, "pageNumber");

            return new CalendarOfItemsTabViewModel(label, description, correctedUrl, query.ViewModes.HasFlag(mode));
        }

        var tabOptions = new[]
        {
            BuildTab(CalendarOfItemsViewModes.Forward, "Upcoming tasks"),
            BuildTab(CalendarOfItemsViewModes.Backward, "Previous tasks"),
        };
        var selectedTab = tabOptions.FirstOrDefault(x => x.IsSelected);
        var lastUpdatedMessage =
            result.Value.Payload.Count > 0
                ? $"Last updated: {result.Value.Payload.Select(x => x.LastUpdated).OfType<DateTime>().Max().ToGdsDateString()}"
                : null;

        var options = new CalendarOfItemViewOptions
        {
            ViewMode = query.ViewModes,
            Tabs = tabOptions.ToCollection(),
            Title = selectedTab?.Label ?? string.Empty,
            Description = selectedTab?.Description ?? string.Empty,
            Heading = "Calendar of tasks",
            SubHeading = "These are all of the required tasks that you must complete for your school each month.",
            GroupingFunction = x => x.SortDate?.ToString("MMMMM yyyy", null)!,
            NoResultsMessage = "No results found",
            LastUpdatedMessage = lastUpdatedMessage,
        };

        return Build(options, result.Value);
    }

    public async Task<CalendarOfItemsViewModel> BuildForDashboard(
        CalendarOfItemsCustomQuery query,
        CancellationToken cancellationToken
    )
    {
        var result = await customQueryBuilder.Handle(query, cancellationToken);

        if (result.IsFailure)
        {
            throw new ApplicationException(result.Error.Description);
        }

        if (result.Value.Payload.Count == 0)
        {
            return Build(new CalendarOfItemViewOptions { NoResultsMessage = "No tasks found" }, result.Value);
        }

        var lastUpdatedDate = result.Value.Payload.Select(x => x.LastUpdated).OfType<DateTime>().Max();

        var options = new CalendarOfItemViewOptions
        {
            ViewMode = CalendarOfItemsViewModes.Custom | CalendarOfItemsViewModes.Standalone,
            Tabs = [],
            Title = "Upcoming tasks",
            Description = "These are all the required tasks that you must complete for your school each month.",
            GroupingFunction = x => x.SortDate?.ToString("MMMMM yyyy", null)!,
            LastUpdatedMessage = $"Last updated: {lastUpdatedDate.ToGdsDateString()}",
            NoResultsMessage = query.NoResultMessage,
        };

        return Build(options, result.Value);
    }
}
