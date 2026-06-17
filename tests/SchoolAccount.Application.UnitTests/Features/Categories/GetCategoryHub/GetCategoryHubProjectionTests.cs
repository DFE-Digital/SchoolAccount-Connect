using AwesomeAssertions;
using SchoolAccount.Application.Features.Categories.GetCategoryHub;
using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.Types;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.Categories.CategoryBuilder;
using static SchoolAccount.Tests.Common.Builders.Tasks.TaskBuilder;

namespace SchoolAccount.Application.UnitTests.Features.Categories.GetCategoryHub;

public class GetCategoryHubProjectionTests
{
    private readonly Func<TypeEntity, GetCategoryHubResponse> _projectCategory = GetCategoryHubProjection
        .ToCategoryHubResponse(1, 10)
        .Compile();

    [Fact]
    public void Projection_of_type_entity_preserves_all_category_properties()
    {
        // Arrange
        var typeEntity = ACategory()
            .WithId(123)
            .Named("statutory-accounts")
            .WithDisplayName("Statutory Accounts")
            .WithDescription("Annual statutory accounts")
            .WithHubViewDescription("Statutory accounts hub view description.")
            .Build();

        // Act
        var result = _projectCategory(typeEntity);

        // Assert
        result.Id.Should().Be(typeEntity.Id);
        result.Name.Should().Be(typeEntity.Name);
        result.DisplayName.Should().Be(typeEntity.DisplayName);
        result.Description.Should().Be(typeEntity.Description);
        result.HubViewDescription.Should().Be(typeEntity.HubViewDescription);
    }

    [Fact]
    public void Projection_maps_nulls_to_non_required_properties_when_empty()
    {
        // Arrange
        var typeEntity = ACategory().WithId(2).Named("CorporationTax").WithDisplayName("Corporation Tax");

        // Act
        var result = _projectCategory(typeEntity);

        // Assert
        result.Description.Should().BeNull();
        result.HubViewDescription.Should().BeNull();
        result.Children.Should().BeEmpty();
        result.Tasks.Items.Should().BeEmpty();
    }

    [Fact]
    public void Projection_maps_children_id_and_display_name()
    {
        // Arrange
        var typeEntity = ACategory()
            .WithId(3)
            .Named("CorporationTax")
            .WithDisplayName("Corporation Tax")
            .WithChild(ACategory().WithId(101).WithDisplayName("A child category"));

        // Act
        var result = _projectCategory(typeEntity);

        // Assert
        result.Children.Should().HaveCount(1);
        result.Children.Should().ContainEquivalentOf(new { Id = 101, Name = "A child category" });
    }

    [Fact]
    public void Projection_maps_multiple_children()
    {
        // Arrange
        var typeEntity = ACategory()
            .WithId(3)
            .Named("CorporationTax")
            .WithDisplayName("Corporation Tax")
            .WithChildren(
                ACategory().WithId(101).WithDisplayName("A child category"),
                ACategory().WithId(102).WithDisplayName("Another child category")
            );

        // Act
        var result = _projectCategory(typeEntity);

        // Assert
        result.Children.Should().HaveCount(2);
        result.Children.Should().ContainEquivalentOf(new { Id = 101, Name = "A child category" });
        result.Children.Should().ContainEquivalentOf(new { Id = 102, Name = "Another child category" });
    }

    [Fact]
    public void Projection_maps_a_task()
    {
        // Arrange
        var typeEntity = ACategory()
            .WithId(3)
            .Named("CorporationTax")
            .WithDisplayName("Corporation Tax")
            .WithTask(
                ATask()
                    .WithId(101)
                    .Named("A task")
                    .WithDescription("A task description")
                    .WithRequirement(Requirement.Mandatory)
            );

        // Act
        var result = _projectCategory(typeEntity);

        // Assert
        result.Tasks.Items.Should().HaveCount(1);
        result
            .Tasks.Items.Should()
            .ContainEquivalentOf(
                new
                {
                    Id = 101,
                    Name = "A task",
                    Description = "A task description",
                    Requirement = Requirement.Mandatory,
                }
            );
    }

    [Fact]
    public void Projection_maps_multiple_tasks_with_all_properties()
    {
        // Arrange
        var task1 = ATask()
            .WithId(101)
            .Named("Submit VAT Return")
            .WithDescription("Complete and submit quarterly VAT return")
            .WithRequirement(Requirement.Mandatory);

        var task2 = ATask()
            .WithId(102)
            .Named("Review Tax Position")
            .WithDescription("Optional review of current tax position")
            .WithRequirement(Requirement.Optional);

        var typeEntity = ACategory()
            .WithId(3)
            .Named("CorporationTax")
            .WithDisplayName("Corporation Tax")
            .WithTasks(task1, task2);

        // Act
        var result = _projectCategory(typeEntity);

        // Assert
        result
            .Tasks.Items.Should()
            .HaveCount(2)
            .And.BeEquivalentTo([
                new
                {
                    Id = 101,
                    Name = "Submit VAT Return",
                    Description = "Complete and submit quarterly VAT return",
                    Requirement = Requirement.Mandatory,
                },
                new
                {
                    Id = 102,
                    Name = "Review Tax Position",
                    Description = "Optional review of current tax position",
                    Requirement = Requirement.Optional,
                },
            ]);
    }
}
