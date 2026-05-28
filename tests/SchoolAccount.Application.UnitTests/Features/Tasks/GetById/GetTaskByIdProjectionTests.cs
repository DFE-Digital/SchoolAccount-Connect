using AwesomeAssertions;
using SchoolAccount.Application.Features.Tasks.GetById;
using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.SchoolTypes;
using SchoolAccount.Domain.Tasks;
using SchoolAccount.Kernel;
using Xunit;
using static SchoolAccount.Domain.Common.WorkflowState;
using static SchoolAccount.Tests.Common.Builders.ResourceBuilder;
using static SchoolAccount.Tests.Common.Builders.SubTaskBuilder;
using static SchoolAccount.Tests.Common.Builders.TaskBuilder;

namespace SchoolAccount.Application.UnitTests.Features.Tasks.GetById;

public class GetTaskByIdProjectionTests
{
    private static Func<TaskEntity, GetTaskByIdResponse> _project(IEnumerable<SchoolTypeTagMappingEntity> mapping)
    {
        return GetTaskByIdProjection.ToTaskResponse(mapping.AsQueryable(), SchoolType.Unknown).Compile();
    }

    [Fact]
    public void Projection_of_task_entity_preserves_all_task_properties()
    {
        // Arrange
        var updatedAt = new DateTime(2026, 4, 21, 10, 0, 0);
        var taskEntity = ATask()
            .WithId(123)
            .Named("Statutory Accounts")
            .WithReferenceNo("TASK-001")
            .WithRequirement(Requirement.Mandatory)
            .UpdatedBy("John Doe")
            .UpdatedAt(2026, 4, 21, 10)
            .WithSubTask(ASubTask().UpdatedAt(updatedAt).InState(Published))
            .Build();

        // Act
        var result = _project([])(taskEntity);

        // Assert
        result.Id.Should().Be(taskEntity.Id);
        result.ReferenceNo.Should().Be(taskEntity.ReferenceNo);
        result.Name.Should().Be(taskEntity.Name);
        result.Requirement.Should().Be(taskEntity.Requirement);
        result.DateUpdated.Should().Be(taskEntity.DateUpdated);
        result.UpdatedBy.Should().Be(taskEntity.UpdatedBy);
        result.SubTaskLastUpdated.Should().Be(updatedAt);
        result.SubTasks.Should().HaveCount(1);
    }

    [Fact]
    public void Published_subtasks_are_included_in_subtasks()
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
        var result = _project([])(taskEntity);

        // Assert
        result
            .SubTasks.Should()
            .ContainSingle()
            .Which.Should()
            .BeEquivalentTo(
                new
                {
                    Id = 1L,
                    Name = "Published SubTask",
                    WorkflowState = Published,
                },
                options => options.ExcludingMissingMembers()
            );
    }

    [Fact]
    public void Expired_subtasks_are_included_in_subtasks()
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
        var result = _project([])(taskEntity);

        // Assert
        result
            .SubTasks.Should()
            .ContainSingle()
            .Which.Should()
            .BeEquivalentTo(
                new
                {
                    Id = 2L,
                    Name = "Expired SubTask",
                    WorkflowState = Expired,
                },
                options => options.ExcludingMissingMembers()
            );
    }

    [Fact]
    public void Draft_subtasks_are_excluded_from_subtasks()
    {
        // Arrange
        var taskEntity = ATask()
            .WithSubTasks(
                ASubTask().WithId(1).InState(Published).WithStartDate(2026, 5, 1).WithDueDate(2026, 6, 1),
                ASubTask().WithId(2).InState(Draft).WithStartDate(2026, 5, 1).WithDueDate(2026, 6, 1)
            )
            .Build();

        // Act
        var result = _project([])(taskEntity);

        // Assert
        result.SubTasks.Should().ContainSingle();
        result.SubTasks.First().Id.Should().Be(1);
    }

    [Fact]
    public void Archived_subtasks_are_excluded_from_subtasks()
    {
        // Arrange
        var taskEntity = ATask()
            .WithSubTasks(
                ASubTask().WithId(1).InState(Published).WithStartDate(2026, 5, 1).WithDueDate(2026, 6, 1),
                ASubTask().WithId(2).InState(Archived).WithStartDate(2026, 5, 1).WithDueDate(2026, 6, 1)
            )
            .Build();

        // Act
        var result = _project([])(taskEntity);

        // Assert
        result.SubTasks.Should().ContainSingle();
        result.SubTasks.First().Id.Should().Be(1);
    }

    [Fact]
    public void Projection_of_subtask_entity_preserves_all_subtask_properties()
    {
        // Arrange
        var subTask = ASubTask()
            .WithId(123)
            .WithReferenceNo("REF-001")
            .Named("Test SubTask")
            .WithDescription("Test Description")
            .WithStartDate(2026, 5, 1, isExact: true)
            .WithDueDate(2026, 6, 1, isExact: false)
            .WithRequirement(Requirement.Mandatory)
            .WithResources(AResource().Named("Infant Formula").WithLink("https://example.com/infant-formula"))
            .InState(Published)
            .UpdatedBy("John Doe")
            .UpdatedAt(2026, 4, 20, 15, 30)
            .Build();

        // Act
        var result = _project([])(ATask().WithSubTask(subTask).Build());

        // Assert
        result
            .SubTasks.Should()
            .ContainSingle()
            .Which.Should()
            .BeEquivalentTo(
                new GetTaskByIdResponseSubtask
                {
                    Id = subTask.Id,
                    ReferenceNo = subTask.ReferenceNo,
                    Name = subTask.Name,
                    Description = subTask.Description,
                    StartDate = subTask.StartDate,
                    StartDateIsExact = subTask.StartDateIsExact,
                    DueDate = subTask.DueDate,
                    DueDateIsExact = subTask.DueDateIsExact,
                    Requirement = subTask.Requirement,
                    WorkflowState = subTask.WorkflowState,
                    DateUpdated = subTask.DateUpdated,
                    UpdatedBy = subTask.UpdatedBy,
                    ResourceName = subTask.Resources.FirstOrDefault()?.ResourceName,
                    ResourceLink = subTask.Resources.FirstOrDefault()?.DigitalLink,
                },
                options => options.Excluding(x => x.AvailabilityLabel).Excluding(x => x.DueDateLabel)
            );
    }

    [Fact]
    public void Subtask_with_no_resources_has_null_resource_name_and_link()
    {
        // Arrange
        var taskEntity = ATask()
            .WithSubTask(ASubTask().WithId(1).InState(Published).WithStartDate(2026, 5, 1).WithDueDate(2026, 6, 1))
            .Build();

        // Act
        var result = _project([])(taskEntity);

        // Assert
        result.SubTasks.First().ResourceName.Should().BeNull();
        result.SubTasks.First().ResourceLink.Should().BeNull();
    }

    [Fact]
    public void Subtask_with_multiple_resources_takes_first_resource_name_and_link()
    {
        // Arrange
        var subTask = ASubTask()
            .WithId(1)
            .InState(Published)
            .WithStartDate(2026, 5, 1)
            .WithDueDate(2026, 6, 1)
            .WithResources(
                AResource().Named("First Resource").WithLink("https://example.com/first"),
                AResource().Named("Second Resource").WithLink("https://example.com/second")
            )
            .Build();

        var taskEntity = ATask().Build();
        taskEntity.SubTasks.Add(subTask);

        // Act
        var result = _project([])(taskEntity);

        // Assert
        result.SubTasks.First().ResourceName.Should().Be("First Resource");
        result.SubTasks.First().ResourceLink.Should().Be("https://example.com/first");
    }

    [Fact]
    public void Multiple_published_and_expired_subtasks_are_all_included()
    {
        // Arrange
        var taskEntity = ATask()
            .WithSubTasks(
                ASubTask().WithId(1).WithDueDate(2026, 6, 1).InState(Published).WithStartDate(2026, 5, 1),
                ASubTask().WithId(2).WithDueDate(2026, 6, 15).InState(Published).WithStartDate(2026, 5, 1),
                ASubTask().WithId(3).WithDueDate(2026, 3, 15).InState(Expired).WithStartDate(2026, 3, 1)
            )
            .Build();

        // Act
        var result = _project([])(taskEntity);

        // Assert
        result.SubTasks.Should().HaveCount(3);
        result.SubTasks.Select(x => x.Id).Should().Contain([1L, 2L, 3L]);
    }

    [Fact]
    public void Task_with_no_subtasks_returns_empty_subtasks_collection()
    {
        // Arrange
        var taskEntity = ATask().Build();

        // Act
        var result = _project([])(taskEntity);

        // Assert
        result.SubTasks.Should().BeEmpty();
    }

    [Fact]
    public void Task_with_only_draft_subtasks_returns_empty_subtasks_collection()
    {
        // Arrange
        var taskEntity = ATask()
            .WithSubTasks(
                ASubTask().WithId(1).InState(Draft).WithStartDate(2026, 5, 1).WithDueDate(2026, 6, 1),
                ASubTask().WithId(2).InState(Draft).WithStartDate(2026, 5, 1).WithDueDate(2026, 6, 1)
            )
            .Build();

        // Act
        var result = _project([])(taskEntity);

        // Assert
        result.SubTasks.Should().BeEmpty();
    }

    [Fact]
    public void Resource_links_and_names_are_mapped()
    {
        // Arrange
        var taskEntity = ATask()
            .WithResources(
                AResource().Named("Infant Formula").WithLink("https://example.com/infant-formula"),
                AResource().Named("Health Declaration").WithLink("https://example.com/health")
            )
            .Build();

        // Act
        var result = _project([])(taskEntity);

        // Assert
        result.Resources.Should().HaveCount(2);
        result
            .Resources.Should()
            .ContainEquivalentOf(new { Name = "Infant Formula", Link = "https://example.com/infant-formula" });
        result
            .Resources.Should()
            .ContainEquivalentOf(new { Name = "Health Declaration", Link = "https://example.com/health" });
    }

    [Fact]
    public void Task_with_no_resources_returns_empty_resources_collection()
    {
        // Arrange
        var taskEntity = ATask().Build();

        // Act
        var result = _project([])(taskEntity);

        // Assert
        result.Resources.Should().BeEmpty();
    }

    [Fact]
    public void Related_tasks_are_mapped()
    {
        // Arrange
        var taskEntity = ATask().Build();
        taskEntity.RelatedTasks.Add(ATask().WithId(10).Named("Related Task 1").Build());
        taskEntity.RelatedTasks.Add(ATask().WithId(20).Named("Related Task 2").Build());

        // Act
        var result = _project([])(taskEntity);

        // Assert
        result.RelatedTasks.Should().HaveCount(2);
        result.RelatedTasks.Should().ContainEquivalentOf(new { Id = 10L, Name = "Related Task 1" });
        result.RelatedTasks.Should().ContainEquivalentOf(new { Id = 20L, Name = "Related Task 2" });
    }

    [Fact]
    public void Task_with_no_related_tasks_returns_empty_related_tasks_collection()
    {
        // Arrange
        var taskEntity = ATask().Build();

        // Act
        var result = _project([])(taskEntity);

        // Assert
        result.RelatedTasks.Should().BeEmpty();
    }
}
