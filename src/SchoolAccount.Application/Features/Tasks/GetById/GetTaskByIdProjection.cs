using System.Linq.Expressions;
using SchoolAccount.Domain.Tasks;
using static SchoolAccount.Domain.Common.WorkflowState;

namespace SchoolAccount.Application.Features.Tasks.GetById;

public static class GetTaskByIdProjection
{
    public static Expression<Func<TaskEntity, GetTaskByIdResponse>> ToTaskResponse()
    {
        return x => new GetTaskByIdResponse
        {
            Id = x.Id,
            ReferenceNo = x.ReferenceNo,
            Name = x.Name,
            Requirement = x.Requirement,
            DateUpdated = x.DateUpdated,
            UpdatedBy = x.UpdatedBy,
            SubTasks = x
                .SubTasks.Where(subtask => subtask.WorkflowState == Published || subtask.WorkflowState == Expired)
                .Select(st => new GetTaskByIdResponseSubtask
                {
                    Id = st.Id,
                    ReferenceNo = st.ReferenceNo,
                    Name = st.Name,
                    Description = st.Description,
                    StartDate = st.StartDate,
                    StartDateIsExact = st.StartDateIsExact,
                    DueDate = st.DueDate,
                    DueDateIsExact = st.DueDateIsExact,
                    Requirement = st.Requirement,
                    WorkflowState = st.WorkflowState,
                    DateUpdated = st.DateUpdated,
                    UpdatedBy = st.UpdatedBy,
                    // The database has a many to many relationship between SubTasks and Resources but manage enforces that
                    // only one resource is allowed per subtask. So we can assume that the first resource is the one we want.
                    ResourceName = st.Resources.Select(r => r.ResourceName).FirstOrDefault(),
                    ResourceLink = st.Resources.Select(r => r.DigitalLink).FirstOrDefault(),
                })
                .ToArray(),
            Resources = x
                .Resources.Select(r => new GetTaskByIdResponseResource { Name = r.ResourceName, Link = r.DigitalLink })
                .ToArray(),
            RelatedTasks = x
                .RelatedTasks.Select(rt => new GetTaskByIdResponseRelatedTask { Id = rt.Id, Name = rt.Name })
                .ToArray(),
            TaskTypes = x
                .TypeTaskMappings.Where(t => t.Type.ParentTypeId == null && t.Type.TypeGroupingId == 1)
                .Select(t => new GetTaskByIdResponseTaskType { Id = t.Type.Id, Name = t.Type.Name })
                .ToArray(),
        };
    }
}
