using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Kernel;
using static SchoolAccount.Domain.Common.WorkflowState;

namespace SchoolAccount.Application.Features.Tasks.GetById;

public sealed class GetTaskByIdHandler(IApplicationDbContext applicationDbContext, IDateTimeProvider dateTimeProvider)
    : IQueryHandler<GetTaskByIdQuery, TaskResponse>
{
    public async Task<Result<TaskResponse>> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken)
    {
        var task = await applicationDbContext
            .Tasks.AsNoTracking()
            .Include(t => t.SubTasks.Where(st => st.WorkflowState == Published || st.WorkflowState == Expired))
            .FirstOrDefaultAsync(t => t.Id == query.Id, cancellationToken);

        if (task == null)
        {
            return new TaskResponse();
        }

        var mapper = new GetTaskByIdMapper(dateTimeProvider);
        var taskResponse = mapper.ToTaskResponse(task, query.ViewMode);

        return Result.Success(taskResponse);
    }
}
