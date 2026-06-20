using SchoolAccount.Application;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Domain.Common;

namespace SchoolAccount.Web.Connect.Features.Calendars.CalendarList;

public class CalendarListRowViewBuilder
{
    public CalendarListRowViewModel Build(CalendarListViewOptions options, CalendarOfItemsRow row)
    {
        var url = DetermineUri(
            row.Type,
            row.Id,
            row.Status?.EntityId == (int)WorkflowState.Expired ? "#previous" : string.Empty
        );

        return new CalendarListRowViewModel(row.Name, url)
        {
            Description = row.Description,
            DateText = !options.ViewMode.HasFlag(CalendarOfItemsViewModes.Hub)
                ? GenerateAvailableOrDueMessage(row)
                : null,
        };
    }

    private static string? GenerateAvailableOrDueMessage(CalendarOfItemsRow row, DateOnly? today = null)
    {
        today ??= DateTime.Today.ToDateOnly();

        string FormatDate(DateOnly date, bool? isExact)
        {
            return date.ToString(
                isExact == true ? FormattingConstants.DateMonthYearFormat : FormattingConstants.MonthYearFormat,
                null
            );
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
}
