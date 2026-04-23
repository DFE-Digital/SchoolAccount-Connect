using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Category.Models;
using SchoolAccount.Domain.Types;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Category.Query;

public sealed class GetCategoryByIdQueryHandler(IApplicationDbContext applicationDbContext)
    : IQueryHandler<GetCategoryByIdQuery, CategoryType>
{
    public async Task<Result<CategoryType>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        var category = await applicationDbContext
            .Types.AsNoTracking()
            .Include(x => x.TypeGrouping)
            .Include(x => x.Children)
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (category is null)
        {
            return Result.Failure<CategoryType>(
                Error.NotFound(nameof(Category), "Could not find a category with the provided identifier.")
            );
        }

        return Result.Success(MapCategoryType(category));
    }

    private static CategoryType MapCategoryType(TypeEntity category)
    {
        return new CategoryType
        {
            Id = category.Id,
            Name = category.Name,
            DisplayName = category.DisplayName,
            Description = category.Description,
            HubViewDescription = category.HubViewDescription,
            ParentTypeId = category.ParentTypeId,
            TypeGrouping = category.TypeGrouping is null
                ? null
                : new CategoryTypeGrouping
                {
                    Id = category.TypeGrouping.Id,
                    Name = category.TypeGrouping.Name,
                    DisplayName = category.TypeGrouping.DisplayName,
                    TypeLevel = category.TypeGrouping.TypeLevel,
                    IsMandatory = category.TypeGrouping.IsMandatory,
                    IsMultiSelect = category.TypeGrouping.IsMultiSelect,
                },
            Children = category.Children.Select(x => x.Id).ToList(),
        };
    }
}
