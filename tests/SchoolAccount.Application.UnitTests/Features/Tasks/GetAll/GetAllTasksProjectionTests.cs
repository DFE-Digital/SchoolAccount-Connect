using AwesomeAssertions;
using SchoolAccount.Application.Features.Tasks.GetAll;
using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.Tasks;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.Tasks.TaskBuilder;

namespace SchoolAccount.Application.UnitTests.Features.Tasks.GetAll;

public class GetAllTasksProjectionTests
{
    private readonly Func<TaskEntity, GetAllTasksResponseTasks> _project = GetAllTasksProjection
        .ToGetAllTasksResponseTasks()
        .Compile();

    [Fact]
    public void Projection_of_task_entity_preserves_all_task_properties()
    {
        // Arrange
        var taskEntity = ATask()
            .WithId(42)
            .Named("Statutory Accounts")
            .WithDescription("Test task description.")
            .WithRequirement(Requirement.Mandatory)
            .Build();

        // Act
        var result = _project(taskEntity);

        // Assert
        result.Id.Should().Be(42);
        result.Name.Should().Be("Statutory Accounts");
        result.Description.Should().Be("Test task description.");
        result.Requirement.Should().Be(Requirement.Mandatory);
    }

    [Fact]
    public void Projection_maps_optional_requirement()
    {
        // Arrange
        var taskEntity = ATask().WithRequirement(Requirement.Optional).Build();

        // Act
        var result = _project(taskEntity);

        // Assert
        result.Requirement.Should().Be(Requirement.Optional);
    }

    [Fact]
    public void Projection_maps_task_with_null_description()
    {
        // Arrange
        var taskEntity = ATask().Build();

        // Act
        var result = _project(taskEntity);

        // Assert
        result.Description.Should().BeNull();
    }

    [Fact]
    public void Multiple_tasks_each_project_independently()
    {
        // Arrange
        var task1 = ATask()
            .WithId(1)
            .Named("Task One")
            .WithDescription("This task has a description.")
            .WithRequirement(Requirement.Mandatory)
            .Build();
        var task2 = ATask().WithId(2).Named("Task Two").WithRequirement(Requirement.Optional).Build();

        // Act
        var result1 = _project(task1);
        var result2 = _project(task2);

        // Assert
        result1.Id.Should().Be(1);
        result1.Name.Should().Be("Task One");
        result1.Description.Should().Be("This task has a description.");
        result1.Requirement.Should().Be(Requirement.Mandatory);

        result2.Id.Should().Be(2);
        result2.Name.Should().Be("Task Two");
        result2.Description.Should().BeNull();
        result2.Requirement.Should().Be(Requirement.Optional);
    }

    [Fact]
    public void Projected_result_is_equivalent_to_expected_response_shape()
    {
        // Arrange
        var taskEntity = ATask()
            .WithId(99)
            .Named("Corporation Tax Return")
            .WithDescription("This task has a description.")
            .WithRequirement(Requirement.Mandatory)
            .Build();

        // Act
        var result = _project(taskEntity);

        // Assert
        result
            .Should()
            .BeEquivalentTo(
                new GetAllTasksResponseTasks
                {
                    Id = 99,
                    Name = "Corporation Tax Return",
                    Description = "This task has a description.",
                    Requirement = Requirement.Mandatory,
                }
            );
    }
}
