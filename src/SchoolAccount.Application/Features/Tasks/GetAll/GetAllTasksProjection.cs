using System.Linq.Expressions;
using SchoolAccount.Domain.Tasks;

namespace SchoolAccount.Application.Features.Tasks.GetAll;

public static class GetAllTasksProjection
{
    public static Expression<Func<TaskEntity, GetAllTasksResponseTask>> ToGetAllTasksResponseTasks()
    {
        return x => new GetAllTasksResponseTask
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Requirement = x.Requirement,
        };
    }
}
