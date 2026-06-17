using System.Linq.Expressions;
using SchoolAccount.Domain.Tasks;

namespace SchoolAccount.Application.Features.Tasks.Search;

public static class SearchTasksProjection
{
    public static Expression<Func<TaskEntity, SearchTasksResponseTask>> ToSearchTasksResponseTask()
    {
        return x => new SearchTasksResponseTask
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            DateUpdated = x.DateUpdated,
        };
    }
}
