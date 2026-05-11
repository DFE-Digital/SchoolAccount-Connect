using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Features.Tasks.GetById;

public sealed record GetTaskByIdQuery(
    [FromRoute] long Id,
    [FromQuery] TaskViewMode ViewMode = TaskViewMode.UpcomingTasks
) : IQuery<GetTaskByIdResponse>;
