namespace SchoolAccount.Web.Connect.Models.CalendarOfItems;

public record CalendarOfItemsRowItemViewModel(string Name, string Url)
{
    public string? Description { get; init; }
    public string? DateText { get; init; }
    public bool ShowTag { get; init; }
    public string? TagTheme { get; init; }
    public string? TagValue { get; init; }
    public bool HasTag => !string.IsNullOrEmpty(TagValue);
    public string? Source { get; init; }
    public bool HasSource => !string.IsNullOrEmpty(Source);
}
