using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Tasks.GetAll;
using SchoolAccount.Application.Features.Tasks.GetById;

namespace SchoolAccount.Web.Connect.Features.Tasks;

public partial class TasksController(
    IQueryHandler<GetAllTasksQuery, GetAllTasksResponse> allTasksHandler,
    IQueryHandler<GetTaskByIdQuery, GetTaskByIdResponse> taskHandler
) : Controller;
