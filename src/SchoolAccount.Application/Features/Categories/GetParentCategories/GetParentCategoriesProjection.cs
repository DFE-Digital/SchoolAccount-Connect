using System.Linq.Expressions;
using SchoolAccount.Domain.Types;

namespace SchoolAccount.Application.Features.Categories.GetParentCategories;

public static class GetParentCategoriesProjection
{
    public static Expression<Func<TypeEntity, GetParentCategoriesResponseCategory>> ParentCategories() =>
        typeEntity => new GetParentCategoriesResponseCategory
        {
            Id = typeEntity.Id,
            Name = typeEntity.Name,
            DisplayName = typeEntity.DisplayName,
            Description = typeEntity.Description,
            Children = typeEntity
                .Children.Select(c => new GetParentCategoriesResponseChildren { Id = c.Id, Name = c.DisplayName })
                .ToArray(),
        };
}
