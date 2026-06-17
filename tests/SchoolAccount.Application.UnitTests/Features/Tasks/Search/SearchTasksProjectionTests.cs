using AwesomeAssertions;
using SchoolAccount.Application.Features.Tasks.Search;
using SchoolAccount.Domain.Tasks;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.Tasks.TaskBuilder;

namespace SchoolAccount.Application.UnitTests.Features.Tasks.Search;

public class SearchTasksProjectionTests
{
    private readonly Func<TaskEntity, SearchTasksResponseTask> _projectTask = SearchTasksProjection
        .ToSearchTasksResponseTask()
        .Compile();

    [Fact]
    public void Projection_of_task_entity_preserves_all_task_properties()
    {
        // Arrange
        var taskEntity = ATask()
            .WithId(123)
            .Named("corporation-tax-return")
            .WithDescription("File the corporation tax return")
            .UpdatedAt(2026, 5, 1, 10)
            .Build();

        // Act
        var result = _projectTask(taskEntity);

        // Assert
        result.Id.Should().Be(taskEntity.Id);
        result.Name.Should().Be(taskEntity.Name);
        result.Description.Should().Be(taskEntity.Description);
        result.DateUpdated.Should().Be(taskEntity.DateUpdated);
    }

    [Fact]
    public void Projection_maps_null_to_description_when_not_provided()
    {
        // Arrange
        var taskEntity = ATask().WithId(1).Named("corporation-tax-return").Build();

        // Act
        var result = _projectTask(taskEntity);

        // Assert
        result.Description.Should().BeNull();
    }
}
