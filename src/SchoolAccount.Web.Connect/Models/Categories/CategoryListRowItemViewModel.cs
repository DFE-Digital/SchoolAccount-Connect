namespace SchoolAccount.Web.Connect.Models.Categories;

public record CategoryListRowItemViewModel(string Name, string Url)
{
    public string? Description { get; init; }
}
