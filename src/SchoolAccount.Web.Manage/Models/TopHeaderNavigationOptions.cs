namespace SchoolAccount.Web.Manage.Models;

public class TopHeaderNavigationOptions : List<TopHeaderNavigation>;

public class TopHeaderNavigation
{
    public string Label { get; init; } = null!;
    public string Href { get; init; } = null!;
}