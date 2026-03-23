namespace SchoolAccount.Web.Connect.Models.CalendarOfItems;

public record CalendarOfItemsTabViewModel(string Label, string? Description, string Href, bool IsSelected)
{
    public string QueryExtensions { get; init; } = "";
    public bool HasDescription => !string.IsNullOrEmpty(Description);
};
