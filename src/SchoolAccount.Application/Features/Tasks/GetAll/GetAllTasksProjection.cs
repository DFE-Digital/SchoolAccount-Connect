using System.Linq.Expressions;
using SchoolAccount.Domain.Tasks;

namespace SchoolAccount.Application.Features.Tasks.GetAll;

public static class GetAllTasksProjection
{
    public static Expression<Func<TaskEntity, GetAllTasksResponseTasks>> ToGetAllTasksResponseTasks()
    {
        return x => new GetAllTasksResponseTasks
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Requirement = x.Requirement,
        };
    }
}
