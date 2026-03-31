using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Domain.ViewModels;
using SchoolAccount.Web.Connect.Builders.Interfaces;
using SchoolAccount.Web.Connect.Models.CalendarOfItems;

namespace SchoolAccount.Web.Connect.Builders;

public class CalendarOfItemsRowViewBuilder : ICalendarOfItemsRowViewBuilder
{
    private static string DetermineUri(CalendarOfItemsRowType type, long id, string? queryExtensions)
    {
        return type switch
        {
            CalendarOfItemsRowType.Task => string.Format(
                Thread.CurrentThread.CurrentCulture,
                RouteConstants.Task.Index,
                id
            ) + (queryExtensions ?? string.Empty),
            _ => throw new NotSupportedException(),
        };
    }

    private static (bool show, string? value, string? theme, string? queryExtension) DetermineTag(
        CalendarOfItemsViewModes mode,
        CalendarOfItemsRow row
    )
    {
        if (
            row.Status is not null
            && mode == CalendarOfItemsViewModes.Forward
            && row.Status!.Type == CalendarOfItemsRowType.WorkflowState
            && row.Status.EntityId == (int)WorkflowStateValues.Expired
        )
        {
            return (true, row.Status.DisplayValue, row.Status.Theme, "#previous");
        }

        return (false, null, null, null);
    }

    private static string? GenerateAvailableOrDueMessage(
        CalendarOfItemsRow row,
        DateOnly? today = null,
        string dateFormatter = "d MMMM yyyy",
        string monthFormatter = "MMMM yyyy"
    )
    {
        today ??= DateTime.Today.ToDateOnly();

        string FormatDate(DateOnly date, bool? isExact)
        {
            return date.ToString(isExact == true ? dateFormatter : monthFormatter, null);
        }

        if (row.DueDate.HasValue)
        {
            return $"Due {FormatDate(row.DueDate.Value, row.DueDateIsExact)}.";
        }

        if (row.StartDate.HasValue && (!row.DueDate.HasValue || row.DueDate.Value >= today))
        {
            if (!row.DueDate.HasValue || row.StartDate.Value > today)
            {
                return $"Available {FormatDate(row.StartDate.Value, row.StartDateIsExact)}.";
            }

            return "Available now.";
        }

        return null;
    }

    public CalendarOfItemsRowItemViewModel Build(CalendarOfItemsViewModes mode, CalendarOfItemsRow row)
    {
        var (showTag, tagValue, tagTheme, queryExtensions) = DetermineTag(mode, row);
        return new CalendarOfItemsRowItemViewModel(row.Name, DetermineUri(row.Type, row.Id, queryExtensions))
        {
            Description = row.Description,
            ShowTag = showTag,
            TagValue = tagValue,
            TagTheme = tagTheme,
            DateText = GenerateAvailableOrDueMessage(row),
        };
    }
}
