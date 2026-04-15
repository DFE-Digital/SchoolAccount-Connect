
using System.Collections.ObjectModel;

namespace SchoolAccount.Application.Features.Category.Models;

public sealed class CategoryType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string DisplayName { get; set; } = null!;
    
    public IReadOnlyCollection<int> Children { get; set; } = [];

    public string? Description { get; set; }
    
    public string? HubViewDescription { get; set; }

    public int? ParentTypeId { get; set; }

    public CategoryTypeGrouping? TypeGrouping { get; set; }

    public Collection<int> AllCategoryIds => [Id, ..Children];
}