namespace SchoolAccount.Application.Features.Categories.Models;

public class CategoryTypeGrouping
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public int? TypeLevel { get; set; }

    public bool? IsMandatory { get; set; }

    public bool? IsMultiSelect { get; set; }
}
