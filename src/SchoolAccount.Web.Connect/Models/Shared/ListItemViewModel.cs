namespace SchoolAccount.Web.Connect.Models.Shared;

public class ListItemViewModel(string name, string url, string? description = null)
{
    public string Name { get; } = name;
    public string Url { get; } = url;
    public string? Description { get; init; } = description;
}
