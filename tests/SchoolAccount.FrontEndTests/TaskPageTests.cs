using AwesomeAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.TaskDetails;
using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.Dtos;
using SchoolAccount.Domain.ViewModels;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Controllers;
using Xunit;

namespace SchoolAccount.FrontEndTests;

public class TaskPageTests
{
    private List<SubTaskListItemDto> _subTaskListItem { get; set; } = [];
    private TaskListItemDto _taskListItem { get; set; } =
        new TaskListItemDto(22, "testRef22", "TestTask1", "Ken lawrie", new DateTime(2026, 01, 01));

    private IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
    private DateTime _today = new(2026, 03, 01);
    private readonly IQueryHandler<TaskDetailQuery, TaskDetailsViewModel> taskHandleMock = Substitute.For<
        IQueryHandler<TaskDetailQuery, TaskDetailsViewModel>
    >();
    public required TaskController controller { get; set; }

    [Theory]
    [InlineData(TaskDetailViewMode.UpcomingTasks, false, true)]
    [InlineData(TaskDetailViewMode.PreviousTasks, true, false)]
    public async Task CheckLinksAreProperlyBuilt(
        TaskDetailViewMode taskDetailViewMode,
        bool expectedPreviousTabIsSet,
        bool expectedUpcomingTabIsSet
    )
    {
        var expectedPreviousTab = new MenuItemViewModel(
            expectedPreviousTabIsSet,
            "?TaskId=22&TabIndex=PreviousTasks",
            "Previous 12 months"
        );
        var expectedUpcomingTab = new MenuItemViewModel(
            expectedUpcomingTabIsSet,
            "?TaskId=22&TabIndex=UpcomingTasks",
            "Upcoming tasks"
        );

        SetupController(taskDetailViewMode);

        var result = await controller.TaskDetailsPage(
            new TaskDetailQuery(23, taskDetailViewMode),
            new CancellationToken()
        );

        var viewResult = result.Result as ViewResult;
        var model = viewResult?.Model as TaskDetailsViewModel;
        var tabViewModel = model?.TabViewModel;

        tabViewModel?.UpcomingTab.Should().BeEquivalentTo(expectedUpcomingTab);
        tabViewModel?.PreviousTab.Should().BeEquivalentTo(expectedPreviousTab);
    }

    private void SetupController(TaskDetailViewMode taskDetailViewMode)
    {
        var context = new DefaultHttpContext();
        AddSubTaskItem();
        var taskListItemWithSubTaskList = new TaskListItemWithSubTaskList(_taskListItem, _subTaskListItem);
        var taskDetailsViewModel = new TaskDetailsViewModel(
            taskListItemWithSubTaskList,
            taskDetailViewMode,
            _dateTimeProvider
        );

        context.Request.Scheme = "https";
        context.Request.Host = new HostString("test.com", 443);
        context.Request.Path = "/api/test";
        context.Request.QueryString = new QueryString($"?{taskDetailViewMode.ToString()}");

        taskHandleMock.Handle(Arg.Any<TaskDetailQuery>(), Arg.Any<CancellationToken>()).Returns(taskDetailsViewModel);

        _dateTimeProvider.UtcNow.Returns(_today);

        controller = new TaskController(taskHandleMock)
        {
            ControllerContext = new ControllerContext() { HttpContext = context },
        };
    }

    private void AddSubTaskItem()
    {
        _subTaskListItem.Add(
            new SubTaskListItemDto(
                31,
                "testRef31",
                "TestSubTask1",
                "Test description for subtask 1 and test task 1",
                "www.google.com",
                "Ken Lawrie",
                new DateOnly(2026, 02, 01),
                new DateTime(2026, 02, 01),
                new DateOnly(2026, 02, 01),
                Requirement: Requirement.Conditional,
                true,
                true,
                WorkflowState.Expired
            )
        );
    }
}
