namespace SchoolAccount.Web.Connect.Features.Calendar.CalendarList;

public record CalendarOfItemsTabViewModel(string Label, string? Description, Uri Href, bool IsSelected)
{
    public string QueryExtensions { get; init; } = "";
    public bool HasDescription => !string.IsNullOrEmpty(Description);
};
