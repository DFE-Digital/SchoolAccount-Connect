namespace SchoolAccount.Web.Connect.Features.CalendarOfItems;

public record CalendarOfItemsRowItemViewModel(string Name, string Url)
{
    public string? Description { get; init; }
    public string? DateText { get; init; }
    public bool ShowTag { get; init; }
    public string? TagTheme { get; init; }
    public string? TagValue { get; init; }
    public bool HasTag => !string.IsNullOrEmpty(TagValue);
}
