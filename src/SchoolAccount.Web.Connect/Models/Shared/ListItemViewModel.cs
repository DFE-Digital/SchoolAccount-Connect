namespace SchoolAccount.Web.Connect.Models.Shared;

public class ListItemViewModel(string name, string url, bool openInNewTab = false, string? description = null)
{
    public string Name { get; } = name;
    public string Url { get; } = url;
    public bool OpenInNewTab { get; init; } = openInNewTab;
    public string? Description { get; init; } = description;
}
