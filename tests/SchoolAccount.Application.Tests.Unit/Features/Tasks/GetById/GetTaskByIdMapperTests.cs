using AwesomeAssertions;
using NSubstitute;
using SchoolAccount.Application.Features.Tasks.GetById;
using SchoolAccount.Domain.Common;
using SchoolAccount.Kernel;
using Xunit;
using static SchoolAccount.Domain.Common.WorkflowState;
using static SchoolAccount.Tests.Common.Builders.ResourceBuilder;
using static SchoolAccount.Tests.Common.Builders.SubTaskBuilder;
using static SchoolAccount.Tests.Common.Builders.TaskBuilder;

namespace SchoolAccount.Application.Tests.Unit.Features.Tasks.GetById;

public class GetTaskByIdMapperTests
{
    private readonly GetTaskByIdMapper _sut;

    public GetTaskByIdMapperTests()
    {
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(new DateTime(2026, 4, 21, 10, 0, 0));
        _sut = new GetTaskByIdMapper(dateTimeProvider);
    }

    [Fact]
    public void Mapping_task_entity_preserves_all_task_properties()
    {
        // Arrange
        var taskEntity = ATask()
            .WithId(123)
            .Named("Statutory Accounts")
            .WithReferenceNo("TASK-001")
            .WithRequirement(Requirement.Mandatory)
            .UpdatedBy("John Doe")
            .UpdatedAt(2026, 4, 21, 10)
            .WithSubTask(
                ASubTask()
                    .InState(Published)
                    .UpdatedAt(2026, 4, 20, 12)
                    .WithStartDate(2026, 4, 22)
                    .WithDueDate(2026, 4, 30)
            )
            .Build();

        var viewMode = TaskViewMode.UpcomingTasks;

        // Act
        var result = _sut.ToTaskResponse(taskEntity, viewMode);

        // Assert
        result
            .Should()
            .BeEquivalentTo(
                new TaskResponse
                {
                    Id = taskEntity.Id,
                    ReferenceNo = taskEntity.ReferenceNo,
                    Name = taskEntity.Name,
                    SubTaskLastUpdated = taskEntity.SubTaskLastUpdated,
                    ViewMode = viewMode,
                    TotalSubTasks = taskEntity.SubTasks.Count,
                    Requirement = taskEntity.Requirement,
                    DateUpdated = taskEntity.DateUpdated,
                    UpdatedBy = taskEntity.UpdatedBy,
                },
                options =>
                    options
                        .Excluding(x => x.UpcomingSubTasks)
                        .Excluding(x => x.PreviousSubTasks)
                        .Excluding(x => x.CurrentSubTasks)
            );

        result.UpcomingSubTasks.Should().HaveCount(1);
        result.PreviousSubTasks.Should().BeEmpty();
    }

    [Fact]
    public void Published_subtasks_are_mapped_to_upcoming_subtasks()
    {
        // Arrange
        var taskEntity = ATask()
            .WithSubTask(
                ASubTask()
                    .WithId(1)
                    .Named("Published SubTask")
                    .InState(Published)
                    .WithStartDate(2026, 5, 1)
                    .WithDueDate(2026, 6, 1)
            )
            .Build();

        // Act
        var result = _sut.ToTaskResponse(taskEntity, TaskViewMode.UpcomingTasks);

        // Assert
        result
            .UpcomingSubTasks.Should()
            .ContainSingle()
            .Which.Should()
            .BeEquivalentTo(new { Id = 1L, Name = "Published SubTask" }, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void Expired_subtasks_are_mapped_to_previous_subtasks()
    {
        // Arrange
        var taskEntity = ATask()
            .WithSubTask(
                ASubTask()
                    .WithId(2)
                    .Named("Expired SubTask")
                    .InState(Expired)
                    .WithStartDate(2026, 3, 1)
                    .WithDueDate(2026, 3, 15)
            )
            .Build();

        // Act
        var result = _sut.ToTaskResponse(taskEntity, TaskViewMode.PreviousTasks);

        // Assert
        result
            .PreviousSubTasks.Should()
            .ContainSingle()
            .Which.Should()
            .BeEquivalentTo(new { Id = 2L, Name = "Expired SubTask" }, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public void Mapping_subtask_entity_preserves_all_subtask_properties()
    {
        // Arrange
        var subTask = ASubTask()
            .WithId(123)
            .WithReferenceNo("REF-001")
            .Named("Test SubTask")
            .WithDescription("Test Description")
            .WithDigitalLink("https://example.com")
            .WithStartDate(2026, 5, 1, isExact: true)
            .WithDueDate(2026, 6, 1, isExact: false)
            .WithRequirement(Requirement.Mandatory)
            .WithResources(AResource().Named("Infant Formula").WithLink("https://example.com/infant-formula"))
            .InState(Published)
            .UpdatedBy("John Doe")
            .UpdatedAt(2026, 4, 20, 15, 30)
            .Build();

        var taskEntity = ATask().Build();
        taskEntity.SubTasks.Add(subTask);

        // Act
        var result = _sut.ToTaskResponse(taskEntity, TaskViewMode.UpcomingTasks);

        // Assert
        result
            .UpcomingSubTasks.Should()
            .ContainSingle()
            .Which.Should()
            .BeEquivalentTo(
                new TaskResponseSubTask
                {
                    Id = subTask.Id,
                    ReferenceNo = subTask.ReferenceNo,
                    Name = subTask.Name,
                    Description = subTask.Description,
                    DigitalLink = subTask.DigitalTaskLink,
                    StartDate = subTask.StartDate,
                    StartDateIsExact = subTask.StartDateIsExact,
                    DueDate = subTask.DueDate,
                    DueDateIsExact = subTask.DueDateIsExact,
                    AvailabilityLabel = "Available 1 May 2026.",
                    DueDateLabel = "Due Jun 2026.",
                    Requirement = subTask.Requirement,
                    WorkflowState = subTask.WorkflowState,
                    DateUpdated = subTask.DateUpdated,
                    UpdatedBy = subTask.UpdatedBy,
                    HasDescription = subTask.HasDescription,
                    HasLink = subTask.HasLink,
                    IsOptional = subTask.IsOptional,
                    ResourceName = subTask.Resources.FirstOrDefault()?.ResourceName,
                    ResourceLink = subTask.Resources.FirstOrDefault()?.DigitalLink,
                }
            );
    }

    [Fact]
    public void Multiple_published_subtasks_are_all_mapped()
    {
        // Arrange
        var taskEntity = ATask()
            .WithSubTasks(
                ASubTask().WithId(1).WithDueDate(2026, 6, 1).InState(Published).WithStartDate(2026, 5, 1),
                ASubTask().WithId(2).WithDueDate(2026, 6, 15).InState(Published).WithStartDate(2026, 5, 1),
                ASubTask().WithId(3).WithDueDate(2026, 7, 1).InState(Published).WithStartDate(2026, 5, 1)
            )
            .Build();

        // Act
        var result = _sut.ToTaskResponse(taskEntity, TaskViewMode.UpcomingTasks);

        // Assert
        result.UpcomingSubTasks.Should().HaveCount(3);
        result.UpcomingSubTasks.Select(x => x.Id).Should().ContainInOrder(3, 2, 1);
    }

    [Fact]
    public void Multiple_expired_subtasks_are_all_mapped()
    {
        // Arrange
        var taskEntity = ATask()
            .WithSubTasks(
                ASubTask().WithId(1).WithStartDate(2026, 3, 1).WithDueDate(2026, 3, 15).InState(Expired),
                ASubTask().WithId(2).WithStartDate(2026, 2, 1).WithDueDate(2026, 2, 28).InState(Expired)
            )
            .Build();

        // Act
        var result = _sut.ToTaskResponse(taskEntity, TaskViewMode.PreviousTasks);

        // Assert
        result.PreviousSubTasks.Should().HaveCount(2);
        result.PreviousSubTasks.Select(x => x.Id).Should().ContainInOrder(1, 2);
    }

    [Fact]
    public void Availability_label_is_generated_for_each_subtask()
    {
        // Arrange
        var taskEntity = ATask()
            .WithSubTask(ASubTask().InState(Published).WithStartDate(2026, 5, 1).WithDueDate(2026, 6, 1))
            .Build();

        // Act
        var result = _sut.ToTaskResponse(taskEntity, TaskViewMode.UpcomingTasks);

        // Assert
        result.UpcomingSubTasks.First().AvailabilityLabel.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Due_date_label_is_generated_for_each_subtask()
    {
        // Arrange
        var taskEntity = ATask()
            .WithSubTask(ASubTask().InState(Published).WithStartDate(2026, 5, 1).WithDueDate(2026, 6, 1, isExact: true))
            .Build();

        // Act
        var result = _sut.ToTaskResponse(taskEntity, TaskViewMode.UpcomingTasks);

        // Assert
        result.UpcomingSubTasks.First().DueDateLabel.Should().NotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData(TaskViewMode.UpcomingTasks)]
    [InlineData(TaskViewMode.PreviousTasks)]
    public void View_mode_is_preserved_in_response(TaskViewMode viewMode)
    {
        // Arrange
        var taskEntity = ATask().Build();

        // Act
        var result = _sut.ToTaskResponse(taskEntity, viewMode);

        // Assert
        result.ViewMode.Should().Be(viewMode);
    }

    [Fact]
    public void Subtask_last_updated_reflects_most_recently_updated_subtask()
    {
        // Arrange
        var taskEntity = ATask()
            .WithSubTasks(
                ASubTask()
                    .UpdatedAt(2026, 4, 15, 10)
                    .InState(Published)
                    .WithStartDate(2026, 5, 1)
                    .WithDueDate(2026, 6, 1),
                ASubTask()
                    .UpdatedAt(2026, 4, 20, 14, 30)
                    .InState(Published)
                    .WithStartDate(2026, 5, 1)
                    .WithDueDate(2026, 6, 15),
                ASubTask()
                    .UpdatedAt(2026, 4, 18, 9, 15)
                    .InState(Published)
                    .WithStartDate(2026, 5, 1)
                    .WithDueDate(2026, 7, 1)
            )
            .Build();

        // Act
        var result = _sut.ToTaskResponse(taskEntity, TaskViewMode.UpcomingTasks);

        // Assert
        result.SubTaskLastUpdated.Should().Be(new DateTime(2026, 4, 20, 14, 30, 0));
    }

    [Fact]
    public void Resource_links_and_names_are_mapped()
    {
        // Arrange
        var taskEntity = ATask()
            .WithResources(AResource().Named("Infant Formula").WithLink("https://example.com/infant-formula"))
            .Build();

        // Act
        var result = _sut.ToTaskResponse(taskEntity, TaskViewMode.UpcomingTasks);

        // Assert
        result
            .Resources.Should()
            .ContainSingle()
            .Which.Should()
            .BeEquivalentTo(new { Name = "Infant Formula", Link = "https://example.com/infant-formula" });
    }
}
