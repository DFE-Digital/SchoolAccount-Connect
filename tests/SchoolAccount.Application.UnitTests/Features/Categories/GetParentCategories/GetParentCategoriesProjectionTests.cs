using AwesomeAssertions;
using SchoolAccount.Application.Features.Categories.GetParentCategories;
using SchoolAccount.Domain.Types;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.Categories.CategoryBuilder;

namespace SchoolAccount.Application.UnitTests.Features.Categories.GetParentCategories;

public class GetParentCategoriesProjectionTests
{
    private readonly Func<TypeEntity, GetParentCategoriesResponseCategory> _projectCategory =
        GetParentCategoriesProjection.ParentCategories().Compile();

    [Fact]
    public void Projection_of_type_entity_preserves_all_category_properties()
    {
        // Arrange
        var typeEntity = ACategory()
            .WithId(123)
            .Named("statutory-accounts")
            .WithDisplayName("Statutory Accounts")
            .WithDescription("Annual statutory accounts")
            .Build();

        // Act
        var result = _projectCategory(typeEntity);

        // Assert
        result.Id.Should().Be(typeEntity.Id);
        result.Name.Should().Be(typeEntity.Name);
        result.DisplayName.Should().Be(typeEntity.DisplayName);
        result.Description.Should().Be(typeEntity.Description);
    }

    [Fact]
    public void Projection_maps_nulls_to_non_required_properties_when_empty()
    {
        // Arrange
        var typeEntity = ACategory().WithId(2).Named("CorporationTax").WithDisplayName("Corporation Tax").Build();

        // Act
        var result = _projectCategory(typeEntity);

        // Assert
        result.Description.Should().BeNull();
        result.Children.Should().BeEmpty();
    }

    [Fact]
    public void Projection_maps_a_child_id_and_display_name()
    {
        // Arrange
        var typeEntity = ACategory()
            .WithId(3)
            .Named("CorporationTax")
            .WithDisplayName("Corporation Tax")
            .WithChild(ACategory().WithId(101).WithDisplayName("A child category"))
            .Build();

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
            )
            .Build();

        // Act
        var result = _projectCategory(typeEntity);

        // Assert
        result.Children.Should().HaveCount(2);
        result.Children.Should().ContainEquivalentOf(new { Id = 101, Name = "A child category" });
        result.Children.Should().ContainEquivalentOf(new { Id = 102, Name = "Another child category" });
    }
}
