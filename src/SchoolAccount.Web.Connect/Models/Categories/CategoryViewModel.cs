namespace SchoolAccount.Web.Connect.Models.Categories;

public record CategoryViewModel()
{
    public int Id { get; init; }
    public string DisplayName { get; init; } = null!;
    public string? Description { get; init; }
}
