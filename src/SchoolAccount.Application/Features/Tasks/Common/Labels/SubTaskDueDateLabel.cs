using static System.Globalization.CultureInfo;

namespace SchoolAccount.Application.Features.Tasks.Common.Labels;

public static class SubTaskDueDateLabel
{
    public static string Generate(DateOnly? dueDate, bool? isExactDate)
    {
        if (!dueDate.HasValue || !isExactDate.HasValue)
        {
            return string.Empty;
        }

        var dateFormat = isExactDate.Value ? "d MMM yyyy" : "MMM yyyy";

        return $"Due {dueDate.Value.ToString(dateFormat, CurrentCulture)}.";
    }
}
