using System.Linq.Expressions;
using SchoolAccount.Domain.Tasks;
using SchoolAccount.Domain.Types;

namespace SchoolAccount.Application.Features.Categories.GetCategoryHub;

public static class GetCategoryHubProjection
{
    public static Expression<Func<TypeEntity, GetCategoryHubResponseCategory>> ToCategoryHubResponseCategory()
    {
        return x => new GetCategoryHubResponseCategory
        {
            Id = x.Id,
            Name = x.Name,
            DisplayName = x.DisplayName,
            Description = x.Description,
            HubViewDescription = x.HubViewDescription,
            TypeGrouping =
                x.TypeGrouping == null
                    ? null
                    : new GetCategoryHubResponseTypeGrouping
                    {
                        Id = x.TypeGrouping.Id,
                        Name = x.TypeGrouping.Name,
                        DisplayName = x.TypeGrouping.DisplayName,
                        TypeLevel = x.TypeGrouping.TypeLevel,
                        IsMandatory = x.TypeGrouping.IsMandatory,
                        IsMultiSelect = x.TypeGrouping.IsMultiSelect,
                    },
            Children = x
                .Children.Select(c => new GetCategoryHubResponseChildren { Id = c.Id, Name = c.DisplayName })
                .ToArray(),
        };
    }

    public static Expression<Func<TaskEntity, GetCategoryHubResponseTasks>> ToCategoryHubResponseTasks()
    {
        return x => new GetCategoryHubResponseTasks
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Requirement = x.Requirement,
        };
    }
}
