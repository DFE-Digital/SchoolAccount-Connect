namespace SchoolAccount.Application.Features.Categories.Models;

public class CategoryRow
{
    public long? Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }
}
