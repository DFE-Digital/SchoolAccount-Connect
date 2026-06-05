using AwesomeAssertions;
using SchoolAccount.Application.Features.Categories.GetCategoryHub;
using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.Tasks;
using SchoolAccount.Domain.Types;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.Tasks.TaskBuilder;

namespace SchoolAccount.Application.UnitTests.Features.Categories.GetCategoryHub;

public class GetCategoryHubProjectionTests
{
    private readonly Func<TypeEntity, GetCategoryHubResponseCategory> _projectCategory = GetCategoryHubProjection
        .ToCategoryHubResponseCategory()
        .Compile();

    private readonly Func<TaskEntity, GetCategoryHubResponseTasks> _projectTask = GetCategoryHubProjection
        .ToCategoryHubResponseTasks()
        .Compile();

    [Fact]
    public void Projection_of_type_entity_preserves_all_category_properties()
    {
        // Arrange
        var typeEntity = new TypeEntity
        {
            Id = 1,
            TagName = "statutory-accounts",
            Name = "StatutoryAccounts",
            DisplayName = "Statutory Accounts",
            Description = "Annual statutory accounts",
            HubViewDescription = "Hub view description",
        };

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
    public void Projection_with_null_description_and_hub_view_description_maps_nulls()
    {
        // Arrange
        var typeEntity = new TypeEntity
        {
            Id = 2,
            TagName = "corporation-tax",
            Name = "CorporationTax",
            DisplayName = "Corporation Tax",
            Description = null,
            HubViewDescription = null,
        };

        // Act
        var result = _projectCategory(typeEntity);

        // Assert
        result.Description.Should().BeNull();
        result.HubViewDescription.Should().BeNull();
    }

    [Fact]
    public void Projection_with_null_type_grouping_maps_null_type_grouping()
    {
        // Arrange
        var typeEntity = new TypeEntity
        {
            Id = 3,
            TagName = "vat",
            Name = "Vat",
            DisplayName = "VAT",
            TypeGrouping = null,
        };

        // Act
        var result = _projectCategory(typeEntity);

        // Assert
        result.TypeGrouping.Should().BeNull();
    }

    [Fact]
    public void Projection_with_type_grouping_preserves_all_type_grouping_properties()
    {
        // Arrange
        var typeGrouping = new TypeGroupingEntity
        {
            Id = 10,
            Name = "ComplianceGroup",
            DisplayName = "Compliance Group",
            TypeLevel = 2,
            IsMandatory = true,
            IsMultiSelect = false,
        };

        var typeEntity = new TypeEntity
        {
            Id = 4,
            TagName = "payroll",
            Name = "Payroll",
            DisplayName = "Payroll",
            TypeGrouping = typeGrouping,
        };

        // Act
        var result = _projectCategory(typeEntity);

        // Assert
        result.TypeGrouping.Should().NotBeNull();
        result.TypeGrouping!.Id.Should().Be(typeGrouping.Id);
        result.TypeGrouping.Name.Should().Be(typeGrouping.Name);
        result.TypeGrouping.DisplayName.Should().Be(typeGrouping.DisplayName);
        result.TypeGrouping.TypeLevel.Should().Be(typeGrouping.TypeLevel);
        result.TypeGrouping.IsMandatory.Should().Be(typeGrouping.IsMandatory);
        result.TypeGrouping.IsMultiSelect.Should().Be(typeGrouping.IsMultiSelect);
    }

    [Fact]
    public void Projection_with_type_grouping_null_optional_fields_maps_nulls()
    {
        // Arrange
        var typeGrouping = new TypeGroupingEntity
        {
            Id = 11,
            Name = "GroupNoOptionals",
            DisplayName = "Group No Optionals",
            TypeLevel = null,
            IsMandatory = null,
            IsMultiSelect = null,
        };

        var typeEntity = new TypeEntity
        {
            Id = 5,
            TagName = "paye",
            Name = "Paye",
            DisplayName = "PAYE",
            TypeGrouping = typeGrouping,
        };

        // Act
        var result = _projectCategory(typeEntity);

        // Assert
        result.TypeGrouping.Should().NotBeNull();
        result.TypeGrouping!.TypeLevel.Should().BeNull();
        result.TypeGrouping.IsMandatory.Should().BeNull();
        result.TypeGrouping.IsMultiSelect.Should().BeNull();
    }

    [Fact]
    public void Projection_with_no_children_returns_empty_children_array()
    {
        // Arrange
        var typeEntity = new TypeEntity
        {
            Id = 6,
            TagName = "accounts",
            Name = "Accounts",
            DisplayName = "Accounts",
        };

        // Act
        var result = _projectCategory(typeEntity);

        // Assert
        result.Children.Should().BeEmpty();
    }

    [Fact]
    public void Projection_maps_children_id_and_display_name()
    {
        // Arrange
        var typeEntity = new TypeEntity
        {
            Id = 7,
            TagName = "compliance",
            Name = "Compliance",
            DisplayName = "Compliance",
        };

        typeEntity.Children.Add(
            new TypeEntity
            {
                Id = 101,
                TagName = "child-one",
                Name = "ChildOne",
                DisplayName = "Child One",
            }
        );

        typeEntity.Children.Add(
            new TypeEntity
            {
                Id = 102,
                TagName = "child-two",
                Name = "ChildTwo",
                DisplayName = "Child Two",
            }
        );

        // Act
        var result = _projectCategory(typeEntity);

        // Assert
        result.Children.Should().HaveCount(2);
        result.Children.Should().ContainEquivalentOf(new { Id = 101, Name = "Child One" });
        result.Children.Should().ContainEquivalentOf(new { Id = 102, Name = "Child Two" });
    }

    [Fact]
    public void Projection_maps_child_name_from_display_name_not_name()
    {
        // Arrange
        var typeEntity = new TypeEntity
        {
            Id = 8,
            TagName = "filing",
            Name = "Filing",
            DisplayName = "Filing",
        };

        typeEntity.Children.Add(
            new TypeEntity
            {
                Id = 201,
                TagName = "child-tag",
                Name = "InternalName",
                DisplayName = "Friendly Display Name",
            }
        );

        // Act
        var result = _projectCategory(typeEntity);

        // Assert
        result.Children.Single().Name.Should().Be("Friendly Display Name");
    }

    // -------------------------------------------------------------------------
    // ToCategoryHubResponseTasks
    // -------------------------------------------------------------------------

    [Fact]
    public void Projection_of_task_entity_preserves_all_task_properties()
    {
        // Arrange
        var taskEntity = ATask().WithId(123).Named("Statutory Accounts").WithRequirement(Requirement.Mandatory).Build();

        // Act
        var result = _projectTask(taskEntity);

        // Assert
        result.Id.Should().Be(taskEntity.Id);
        result.Name.Should().Be(taskEntity.Name);
        result.Description.Should().Be(taskEntity.Description);
        result.Requirement.Should().Be(taskEntity.Requirement);
    }

    [Fact]
    public void Projection_of_task_maps_optional_requirement()
    {
        // Arrange
        var taskEntity = ATask().WithRequirement(Requirement.Optional).Build();

        // Act
        var result = _projectTask(taskEntity);

        // Assert
        result.Requirement.Should().Be(Requirement.Optional);
    }

    [Fact]
    public void Projection_of_task_with_null_description_maps_null()
    {
        // Arrange
        var taskEntity = ATask().Build();

        // Act
        var result = _projectTask(taskEntity);

        // Assert
        result.Description.Should().BeNull();
    }

    [Fact]
    public void Multiple_tasks_each_project_independently()
    {
        // Arrange
        var task1 = ATask().WithId(1).Named("Task One").WithRequirement(Requirement.Mandatory).Build();
        var task2 = ATask().WithId(2).Named("Task Two").WithRequirement(Requirement.Optional).Build();

        // Act
        var result1 = _projectTask(task1);
        var result2 = _projectTask(task2);

        // Assert
        result1.Id.Should().Be(1);
        result1.Name.Should().Be("Task One");
        result1.Requirement.Should().Be(Requirement.Mandatory);

        result2.Id.Should().Be(2);
        result2.Name.Should().Be("Task Two");
        result2.Requirement.Should().Be(Requirement.Optional);
    }
}
