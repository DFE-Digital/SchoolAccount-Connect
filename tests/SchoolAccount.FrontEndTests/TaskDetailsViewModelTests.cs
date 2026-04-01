using System.Globalization;
using AwesomeAssertions;
using NSubstitute;
using SchoolAccount.Application.Features.TaskDetails.ViewModels;
using SchoolAccount.Domain.Dtos;
using SchoolAccount.Domain.ViewModels;
using SchoolAccount.Kernel;
using Xunit;

namespace SchoolAccount.FrontEndTests.TaskPageTests;

public class TaskDetailsViewModelTests
{
    private TaskListItemDto _taskListItemDto { get; set; } =
        new TaskListItemDto(22, "testRef22", "TestTask1", "Ken lawrie", new DateTime(2026, 01, 01));
    private List<SubTaskListItemDto> _subTaskListItemDto { get; set; } = [];
    private IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
    private DateTime _today = new(2026, 03, 01);

    public TaskDetailsViewModelTests()
    {
        _dateTimeProvider.UtcNow.Returns(_today);
    }

    [Theory]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowStateValues.Published, true, "Available 2 Mar 2026.")]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowStateValues.Expired, true, "Available 2 Mar 2026.")]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowStateValues.Published, false, "Available Mar 2026.")]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowStateValues.Expired, false, "Available Mar 2026.")]
    public async Task CheckAvailableAndDueDateLabelIsSetWhenHasStartDateAndCorrectWorkFlowState(
        TaskDetailViewMode taskDetailViewModes,
        WorkflowStateValues workflowStateValue,
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
        AddSubTaskItem(startDate, dueDate, workflowStateValue, exactDate, dueDateIsExact: false);
        var tasksWithSubtasks = new TaskListItemWithSubTaskList(_taskListItemDto, _subTaskListItemDto);

        var model = new TaskDetailsViewModel(tasksWithSubtasks, taskDetailViewModes, _dateTimeProvider);
        var tabViewModel = model.TabViewModel;
        var subTask = tabViewModel?.SelectedTabSubTasks?.First();

        subTask?.DueDateLabel.Should().Be("Due Mar 2026.");
        subTask?.AvailabilityLabel.Should().Be(expectedAvailable);
    }

    [Theory]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowStateValues.Expired, true)]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowStateValues.Published, true)]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowStateValues.Expired, false)]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowStateValues.Published, false)]
    public async Task CheckSubTasksNotPopulatedWhenNotMatchingViewModeWithWorkFlowState(
        TaskDetailViewMode taskDetailViewModes,
        WorkflowStateValues workflowStateValue,
        bool exactDate
    )
    {
        var dueDate = DateOnly.FromDateTime(
            DateTime.ParseExact("11/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        var startDate = DateOnly.FromDateTime(
            DateTime.ParseExact("02/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        AddSubTaskItem(startDate, dueDate, workflowStateValue, exactDate, dueDateIsExact: false);
        var tasksWithSubtasks = new TaskListItemWithSubTaskList(_taskListItemDto, _subTaskListItemDto);

        var model = new TaskDetailsViewModel(tasksWithSubtasks, taskDetailViewModes, _dateTimeProvider);
        model.LastUpdatedDate.Should().BeEquivalentTo("1 January 2026");
        var tabViewModel = model.TabViewModel;
        tabViewModel?.SelectedTabSubTasks.Count.Should().Be(0);
    }

    [Theory]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowStateValues.Published, null, "No due date")]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowStateValues.Published, false, "Due Mar 2026.")]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowStateValues.Published, true, "Due 11 Mar 2026.")]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowStateValues.Expired, null, "No due date")]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowStateValues.Expired, false, "Due Mar 2026.")]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowStateValues.Expired, true, "Due 11 Mar 2026.")]
    public async Task CheckDueDateReturnsCorrectFormat(
        TaskDetailViewMode taskDetailViewModes,
        WorkflowStateValues workflowStateValue,
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
        AddSubTaskItem(startDate, dueDate, workflowStateValue, startDateExact: true, dueDateExact);
        var tasksWithSubtasks = new TaskListItemWithSubTaskList(_taskListItemDto, _subTaskListItemDto);

        var model = new TaskDetailsViewModel(tasksWithSubtasks, taskDetailViewModes, _dateTimeProvider);
        var tabViewModel = model.TabViewModel;
        var subTask = tabViewModel?.SelectedTabSubTasks?.First();
        subTask?.DueDateLabel.Should().Be(expectedDueDateLabel);
    }

    [Theory]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowStateValues.Expired, null)]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowStateValues.Expired, false)]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowStateValues.Expired, true)]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowStateValues.Published, null)]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowStateValues.Published, false)]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowStateValues.Published, true)]
    public async Task CheckDueDateReturnsNoSubTasksWhenNotCorrectFormat(
        TaskDetailViewMode taskDetailViewModes,
        WorkflowStateValues workflowStateValue,
        bool? dueDateExact
    )
    {
        var dueDate = DateOnly.FromDateTime(
            DateTime.ParseExact("11/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        var startDate = DateOnly.FromDateTime(
            DateTime.ParseExact("02/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        AddSubTaskItem(startDate, dueDate, workflowStateValue, startDateExact: true, dueDateExact);
        var tasksWithSubtasks = new TaskListItemWithSubTaskList(_taskListItemDto, _subTaskListItemDto);

        var model = new TaskDetailsViewModel(tasksWithSubtasks, taskDetailViewModes, _dateTimeProvider);
        var tabViewModel = model.TabViewModel;
        tabViewModel?.SelectedTabSubTasks.Count.Should().Be(0);
    }

    [Theory]
    [InlineData(
        TaskDetailViewMode.UpcomingTasks,
        WorkflowStateValues.Published,
        true,
        null,
        "12/02/2026",
        "Available Now."
    )]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowStateValues.Published, null, false, "", "Available Now.")]
    [InlineData(
        TaskDetailViewMode.UpcomingTasks,
        WorkflowStateValues.Published,
        false,
        true,
        "01/02/2026",
        "Available Now."
    )]
    [InlineData(
        TaskDetailViewMode.UpcomingTasks,
        WorkflowStateValues.Published,
        true,
        true,
        "02/03/2026",
        "Available 2 Mar 2026."
    )]
    [InlineData(
        TaskDetailViewMode.UpcomingTasks,
        WorkflowStateValues.Published,
        false,
        true,
        "01/03/2026",
        "Available Mar 2026."
    )]
    [InlineData(
        TaskDetailViewMode.PreviousTasks,
        WorkflowStateValues.Expired,
        true,
        true,
        "01/03/2026",
        "Available 1 Mar 2026."
    )]
    [InlineData(
        TaskDetailViewMode.PreviousTasks,
        WorkflowStateValues.Expired,
        false,
        true,
        "01/03/2026",
        "Available Mar 2026."
    )]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowStateValues.Expired, null, true, "", "")]
    public async Task CheckAvailabilityLabelCorrectlyFormatted(
        TaskDetailViewMode taskDetailViewModes,
        WorkflowStateValues workflowStateValue,
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
        AddSubTaskItem(startDate, dueDate, workflowStateValue, startDateExact, dueDateExact);

        var tasksWithSubtasks = new TaskListItemWithSubTaskList(_taskListItemDto, _subTaskListItemDto);

        var model = new TaskDetailsViewModel(tasksWithSubtasks, taskDetailViewModes, _dateTimeProvider);
        var tabViewModel = model.TabViewModel;
        var subTask = tabViewModel?.SelectedTabSubTasks?.First();
        subTask?.AvailabilityLabel.Should().Be(expectedAvailabilityLabel);
    }

    [Theory]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowStateValues.Published, true, "1 January 2026")]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowStateValues.Expired, true, "1 February 2026")]
    [InlineData(TaskDetailViewMode.UpcomingTasks, WorkflowStateValues.Published, false, "1 January 2026")]
    [InlineData(TaskDetailViewMode.PreviousTasks, WorkflowStateValues.Expired, false, "1 February 2026")]
    public async Task CheckLastUpdatedDateIsSetWhenHasStartDateAndCorrectWorkFlowState(
        TaskDetailViewMode taskDetailViewModes,
        WorkflowStateValues workflowStateValue,
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
        AddSubTaskItem(startDate, dueDate, workflowStateValue, exactDate, dueDateIsExact: false);
        var tasksWithSubtasks = new TaskListItemWithSubTaskList(_taskListItemDto, _subTaskListItemDto);

        var model = new TaskDetailsViewModel(tasksWithSubtasks, taskDetailViewModes, _dateTimeProvider);
        var tabViewModel = model.TabViewModel;
        var subTask = tabViewModel?.SelectedTabSubTasks?.First();
        model.LastUpdatedDate?.Should().BeEquivalentTo(expectedLastUpdatedDate);
    }

    private void AddSubTaskItem(
        DateOnly? startDate,
        DateOnly? dueDate,
        WorkflowStateValues workflowState,
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
                RequirementId: 2,
                startDateExact,
                dueDateIsExact,
                (int)workflowState
            )
        );
    }
}
