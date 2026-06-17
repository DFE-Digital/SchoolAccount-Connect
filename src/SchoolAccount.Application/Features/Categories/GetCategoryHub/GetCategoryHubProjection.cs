using System.Linq.Expressions;
using SchoolAccount.Application.Common;
using SchoolAccount.Domain.Types;

namespace SchoolAccount.Application.Features.Categories.GetCategoryHub;

public static class GetCategoryHubProjection
{
    public static Expression<Func<TypeEntity, GetCategoryHubResponse>> ToCategoryHubResponse(
        int pageNumber,
        int pageSize
    )
    {
        return x => new GetCategoryHubResponse
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
            Tasks = new PagedResult<GetCategoryHubResponseTasks>
            {
                Items = x
                    .Tasks.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(t => new GetCategoryHubResponseTasks
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description,
                        Requirement = t.Requirement,
                    })
                    .ToArray(),
                TotalCount = x.Tasks.Count,
                PageNumber = pageNumber,
                PageSize = pageSize,
            },
        };
    }
}
