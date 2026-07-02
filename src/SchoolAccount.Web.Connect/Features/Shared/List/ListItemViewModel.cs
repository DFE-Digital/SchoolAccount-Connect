namespace SchoolAccount.Web.Connect.Features.Shared.List;

public class ListItemViewModel
{
    public ListItemViewModel(string name, string url, bool openInNewTab = false, string? description = null)
    {
        Name = name;
        Url = url;
        OpenInNewTab = openInNewTab;
        Description = description;
    }

    public ListItemViewModel(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public string? Url { get; }

    public bool OpenInNewTab { get; init; }

    public string? Description { get; init; }

    public bool HasUrl => !string.IsNullOrWhiteSpace(Url);
}
