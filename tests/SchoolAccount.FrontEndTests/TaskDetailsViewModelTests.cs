using System.Globalization;
using AwesomeAssertions;
using NSubstitute;
using SchoolAccount.Domain.Dtos;
using SchoolAccount.Domain.Enums;
using SchoolAccount.Domain.ViewModels;
using SchoolAccount.Kernel;
using Xunit;

namespace SchoolAccount.FrontEndTests;

public class TaskDetailsViewModelTests
{
    private TaskListItemDto _taskListItemDto { get; set; } =
        new TaskListItemDto(22, "testRef22", "TestTask1", "Ken Lawrie", new DateTime(2026, 01, 01));
    private List<SubTaskListItemDto> _subTaskListItemDto { get; set; } = [];
    private IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
    private DateTime _today = new(2026, 03, 01);

    public TaskDetailsViewModelTests()
    {
        _dateTimeProvider.UtcNow.Returns(_today);
    }

    [Theory]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowState.Published, true, "Available 2 Mar 2026.")]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowState.Expired, true, "Available 2 Mar 2026.")]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowState.Published, false, "Available Mar 2026.")]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowState.Expired, false, "Available Mar 2026.")]
    public async Task CheckAvailableAndDueDateLabelIsSetWhenHasStartDateAndCorrectWorkFlowState(
        TaskDetailViewMode taskDetailViewModes,
        WorkflowState workflowState,
        bool exactDate,
        string expectedAvailable
    )
    {
        var dueDate = DateOnly.FromDateTime(
            DateTime.ParseExact("11/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        var startDate = DateOnly.FromDateTime(
            DateTime.ParseExact("02/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        AddSubTaskItem(startDate, dueDate, workflowState, exactDate, dueDateIsExact: false);
        var tasksWithSubtasks = new TaskListItemWithSubTaskList(_taskListItemDto, _subTaskListItemDto);

        var model = new TaskDetailsViewModel(tasksWithSubtasks, taskDetailViewModes, _dateTimeProvider);
        var tabViewModel = model.TabViewModel;
        var subTask = tabViewModel?.SelectedTabSubTasks?.First();

        subTask?.DueDateLabel.Should().Be("Due Mar 2026.");
        subTask?.AvailabilityLabel.Should().Be(expectedAvailable);
    }

    [Theory]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowState.Expired, true)]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowState.Published, true)]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowState.Expired, false)]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowState.Published, false)]
    public async Task CheckSubTasksNotPopulatedWhenNotMatchingViewModeWithWorkFlowState(
        TaskDetailViewMode taskDetailViewModes,
        WorkflowState workflowState,
        bool exactDate
    )
    {
        var dueDate = DateOnly.FromDateTime(
            DateTime.ParseExact("11/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        var startDate = DateOnly.FromDateTime(
            DateTime.ParseExact("02/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        AddSubTaskItem(startDate, dueDate, workflowState, exactDate, dueDateIsExact: false);
        var tasksWithSubtasks = new TaskListItemWithSubTaskList(_taskListItemDto, _subTaskListItemDto);

        var model = new TaskDetailsViewModel(tasksWithSubtasks, taskDetailViewModes, _dateTimeProvider);
        model.LastUpdatedDate.Should().BeEquivalentTo("1 January 2026");
        var tabViewModel = model.TabViewModel;
        tabViewModel?.SelectedTabSubTasks.Count.Should().Be(0);
    }

    [Theory]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowState.Published, null, "No due date")]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowState.Published, false, "Due Mar 2026.")]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowState.Published, true, "Due 11 Mar 2026.")]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowState.Expired, null, "No due date")]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowState.Expired, false, "Due Mar 2026.")]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowState.Expired, true, "Due 11 Mar 2026.")]
    public async Task CheckDueDateReturnsCorrectFormat(
        TaskDetailViewMode taskDetailViewModes,
        WorkflowState workflowState,
        bool? dueDateExact,
        string expectedDueDateLabel
    )
    {
        var dueDate = DateOnly.FromDateTime(
            DateTime.ParseExact("11/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        var startDate = DateOnly.FromDateTime(
            DateTime.ParseExact("02/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        AddSubTaskItem(startDate, dueDate, workflowState, startDateExact: true, dueDateExact);
        var tasksWithSubtasks = new TaskListItemWithSubTaskList(_taskListItemDto, _subTaskListItemDto);

        var model = new TaskDetailsViewModel(tasksWithSubtasks, taskDetailViewModes, _dateTimeProvider);
        var tabViewModel = model.TabViewModel;
        var subTask = tabViewModel?.SelectedTabSubTasks?.First();
        subTask?.DueDateLabel.Should().Be(expectedDueDateLabel);
    }

    [Theory]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowState.Expired, null)]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowState.Expired, false)]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowState.Expired, true)]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowState.Published, null)]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowState.Published, false)]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowState.Published, true)]
    public async Task CheckDueDateReturnsNoSubTasksWhenNotCorrectFormat(
        TaskDetailViewMode taskDetailViewModes,
        WorkflowState workflowState,
        bool? dueDateExact
    )
    {
        var dueDate = DateOnly.FromDateTime(
            DateTime.ParseExact("11/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        var startDate = DateOnly.FromDateTime(
            DateTime.ParseExact("02/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        AddSubTaskItem(startDate, dueDate, workflowState, startDateExact: true, dueDateExact);
        var tasksWithSubtasks = new TaskListItemWithSubTaskList(_taskListItemDto, _subTaskListItemDto);

        var model = new TaskDetailsViewModel(tasksWithSubtasks, taskDetailViewModes, _dateTimeProvider);
        var tabViewModel = model.TabViewModel;
        tabViewModel?.SelectedTabSubTasks.Count.Should().Be(0);
    }

    [Theory]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowState.Published, true, null, "12/02/2026", "Available Now.")]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowState.Published, null, false, "", "Available Now.")]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowState.Published, false, true, "01/02/2026", "Available Now.")]
    [InlineData(
        TaskDetailViewMode.UpcomingTasks,
        WorkflowState.Published,
        true,
        true,
        "02/03/2026",
        "Available 2 Mar 2026."
    )]
    [InlineData(
        TaskDetailViewMode.UpcomingTasks,
        WorkflowState.Published,
        false,
        true,
        "01/03/2026",
        "Available Mar 2026."
    )]
    [InlineData(
        TaskDetailViewMode.PreviousTasks,
        WorkflowState.Expired,
        true,
        true,
        "01/03/2026",
        "Available 1 Mar 2026."
    )]
    [InlineData(
        TaskDetailViewMode.PreviousTasks,
        WorkflowState.Expired,
        false,
        true,
        "01/03/2026",
        "Available Mar 2026."
    )]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowState.Expired, null, true, "", "")]
    public async Task CheckAvailabilityLabelCorrectlyFormatted(
        TaskDetailViewMode taskDetailViewModes,
        WorkflowState workflowState,
        bool? startDateExact,
        bool? dueDateExact,
        string startDateString,
        string expectedAvailabilityLabel
    )
    {
        DateOnly? startDate = null;
        if (!string.IsNullOrEmpty(startDateString))
        {
            startDate = DateOnly.FromDateTime(
                DateTime.ParseExact(startDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            );
        }
        var dueDate = DateOnly.FromDateTime(
            DateTime.ParseExact("11/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        AddSubTaskItem(startDate, dueDate, workflowState, startDateExact, dueDateExact);

        var tasksWithSubtasks = new TaskListItemWithSubTaskList(_taskListItemDto, _subTaskListItemDto);

        var model = new TaskDetailsViewModel(tasksWithSubtasks, taskDetailViewModes, _dateTimeProvider);
        var tabViewModel = model.TabViewModel;
        var subTask = tabViewModel?.SelectedTabSubTasks?.First();
        subTask?.AvailabilityLabel.Should().Be(expectedAvailabilityLabel);
    }

    [Theory]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowState.Published, true, "1 January 2026")]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowState.Expired, true, "1 February 2026")]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowState.Published, false, "1 January 2026")]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowState.Expired, false, "1 February 2026")]
    public async Task CheckLastUpdatedDateIsSetWhenHasStartDateAndCorrectWorkFlowState(
        TaskDetailViewMode taskDetailViewModes,
        WorkflowState workflowState,
        bool exactDate,
        string expectedLastUpdatedDate
    )
    {
        var startDate = DateOnly.FromDateTime(
            DateTime.ParseExact("02/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        var dueDate = DateOnly.FromDateTime(
            DateTime.ParseExact("11/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        AddSubTaskItem(startDate, dueDate, workflowState, exactDate, dueDateIsExact: false);
        var tasksWithSubtasks = new TaskListItemWithSubTaskList(_taskListItemDto, _subTaskListItemDto);

        var model = new TaskDetailsViewModel(tasksWithSubtasks, taskDetailViewModes, _dateTimeProvider);
        var tabViewModel = model.TabViewModel;
        var subTask = tabViewModel?.SelectedTabSubTasks?.First();
        model.LastUpdatedDate?.Should().BeEquivalentTo(expectedLastUpdatedDate);
    }

    private void AddSubTaskItem(
        DateOnly? startDate,
        DateOnly? dueDate,
        WorkflowState workflowState,
        bool? startDateExact,
        bool? dueDateIsExact
    )
    {
        _subTaskListItemDto.Add(
            new SubTaskListItemDto(
                31,
                "testRef31",
                "TestSubTask1",
                "Test description for subtask 1 and test task 1",
                "www.google.com",
                "Ken Lawrie",
                startDate,
                new DateTime(2026, 02, 01),
                dueDate,
                Requirement: Requirement.Conditional,
                startDateExact,
                dueDateIsExact,
                workflowState
            )
        );
    }
}
