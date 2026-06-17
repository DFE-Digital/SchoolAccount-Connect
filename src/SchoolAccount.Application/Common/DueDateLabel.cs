using static System.Globalization.CultureInfo;

namespace SchoolAccount.Application.Common;

public static class DueDateLabel
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
