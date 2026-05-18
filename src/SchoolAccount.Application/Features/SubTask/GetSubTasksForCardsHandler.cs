using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Specifications;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.SubTask;

public class GetSubTasksForCardsHandler(
    IOrganisationContext organisationContext, 
    IApplicationDbContext database
) : IQueryHandler<GetSubTasksForCardsQuery, GetSubTasksForCardsResponse>
{
    public async Task<Result<GetSubTasksForCardsResponse>> Handle(GetSubTasksForCardsQuery query, CancellationToken cancellationToken)
    {
        var queryFrom = query.QueryFrom ?? DateOnly.FromDateTime(DateTime.Now.StartOfMonth());
        
        var accessibleTags = database.SchoolTypeTagMappings.AsQueryable();
        var found = await database
            .SubTasks.AsNoTracking()
            .Include(x => x.Task)
            .Include(x => x.TagsSourceMappings)
            .Include(x => x.Conditions)
            .Where(SubTaskEntitySpecifications.IsAccessibleForSchoolType(accessibleTags, organisationContext.Type))
            .Where(x => (x.DueDate ?? x.StartDate) >= queryFrom)
            .Where(x => x.Conditions.Any())
            .Select(x => new GetSubTasksForCardsResponseSubTask
            {
                Id = x.Id,
                ParentId = x.TaskId,
                Name = x.Name,
                Description = x.Description ?? string.Empty,
                Status = new GetSubTasksForCardsResponseNode
                {
                    DisplayValue = Enum.GetName(x.WorkflowState)!,
                    EntityId = (long)x.WorkflowState,
                },
                Runtime = new DateOnlyNullableRange(x.StartDate ?? x.DueDate, x.DueDate),
                LastUpdated = x.DateUpdated,
                Tags = x.TagsSourceMappings
                    .Select(t => new GetSubTasksForCardsResponseNode
                    {
                        Identifier = t.Tag.Id,
                        Name = t.Tag.Name,
                        DisplayValue = t.Tag.Name,
                        Group =  t.Tag.Taxonomy.DisplayName
                    })
                    .ToCollection(),
                Types = x.Task.TypeTaskMappings
                    .Select(t => new GetSubTasksForCardsResponseNode
                    {
                        Identifier = t.Type.Id,
                        Name = t.Type.Name,
                        DisplayValue = t.Type.Name,
                    })
                    .ToCollection(),
                Condition = x.Conditions
                    .Select(c => new GetSubTasksForCardsResponseCondition
                    {
                        Identifier = c.Condition.Identifier,
                        ComparitorType = c.Comparitor,
                        Value = c.Value
                    })
                    .ToCollection()
            })
            .ToListAsync(cancellationToken);

        return Result.Success(
            new GetSubTasksForCardsResponse
            {
                SubTasks = found.ToCollection()
            }
        );
    }
}