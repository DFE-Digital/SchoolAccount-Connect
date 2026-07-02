using MockQueryable.NSubstitute;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Features.Tasks.GetById;
using SchoolAccount.Domain.Resources;
using SchoolAccount.Domain.Sources;
using SchoolAccount.Domain.Tags;
using SchoolAccount.Domain.Tasks;
using SchoolAccount.Kernel;
using SchoolAccount.Tests.Common.Fakes;
using static SchoolAccount.Domain.Common.WorkflowState;
using static SchoolAccount.Tests.Common.Builders.OrganisationContextBuilder;
using static SchoolAccount.Tests.Common.Builders.TagBuilder;
using static SchoolAccount.Tests.Common.Builders.Tasks.SubTaskBuilder;
using static SchoolAccount.Tests.Common.Builders.Tasks.TaskBuilder;
using static SchoolAccount.Tests.Common.Builders.TaxonomyBuilder;

namespace SchoolAccount.Application.IntegrationTests;

public class GetTaskByIdQueryHandlerTests
{
    private readonly IApplicationDbContext _context;
    private readonly GetTaskByIdQueryHandler _sut;

    public GetTaskByIdQueryHandlerTests()
    {
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(new DateTime(2026, 4, 21, 10, 0, 0, DateTimeKind.Utc));

        var organisationContext = AOrganisationContext().WithSchoolType(SchoolType.Academy).Build();

        _context = DatabaseContext.Build();
        _sut = new GetTaskByIdQueryHandler(_context, dateTimeProvider, organisationContext);
    }

    [Fact]
    public async Task Returns_error_response_when_task_not_found()
    {
        // Arrange
        var tasks = Array.Empty<TaskEntity>().BuildMockDbSet();
        var expectedError = GetTaskByIdErrors.NotFound(999);
        var query = new GetTaskByIdQuery(999);

        await _context.Map(
            x =>
            {
                x.Tasks.AddRange(tasks);
            },
            CancellationToken.None
        );

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(expectedError);
    }

    [Fact]
    public async Task Applies_availability_labels_to_all_subtasks()
    {
        // Arrange
        await _context.Map(
            ctx =>
            {
                var taxonomyEntity = ATaxonomy()
                    .WithId(1)
                    .WithName("Institution_Type_Access")
                    .WithDisplayName("Institution types")
                    .IsMandatory()
                    .IsMultiSelect()
                    .WithTags(ATag().WithId((int)SchoolType.Academy).WithName(nameof(SchoolType.Academy)))
                    .Build();
                var taskEntity = ATask()
                    .WithId(1)
                    .WithSubTasks(
                        ASubTask().WithId(1).InState(Published).WithStartDate(2026, 5, 1).WithDueDate(2026, 6, 1),
                        ASubTask().WithId(2).InState(Expired).WithStartDate(2026, 3, 1).WithDueDate(2026, 3, 31)
                    )
                    .Build();
                var subtaskSource = new SourceEntity { Id = (int)Source.Subtask, Name = nameof(Source.Subtask) };

                ctx.Taxonomies.Add(taxonomyEntity);
                ctx.SchoolTypeTagMappings.AddRange(
                    taxonomyEntity.Tags.Select((x, i) => WithSchoolType(x, i + 1, (SchoolType)(i + 1)))
                );
                ctx.Tasks.Add(taskEntity);
                ctx.TagsSourceMappings.AddRange(
                    new TagsSourceMappingEntity
                    {
                        Id = 1,
                        Tag = taxonomyEntity.Tags.ElementAt(0),
                        TagId = taxonomyEntity.Tags.ElementAt(0).Id,
                        SubTask = taskEntity.SubTasks.ElementAt(0),
                        EntityId = taskEntity.SubTasks.ElementAt(0).Id,
                        Source = subtaskSource,
                        SourceId = subtaskSource.Id,
                    },
                    new TagsSourceMappingEntity
                    {
                        Id = 2,
                        Tag = taxonomyEntity.Tags.ElementAt(0),
                        TagId = taxonomyEntity.Tags.ElementAt(0).Id,
                        SubTask = taskEntity.SubTasks.ElementAt(1),
                        EntityId = taskEntity.SubTasks.ElementAt(1).Id,
                        Source = subtaskSource,
                        SourceId = subtaskSource.Id,
                    }
                );
            },
            CancellationToken.None
        );

        var query = new GetTaskByIdQuery(1);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result
            .Value.SubTasks.Should()
            .HaveCount(2)
            .And.AllSatisfy(st =>
            {
                st.AvailabilityLabel.Should().NotBeNullOrEmpty("availability labels should be applied to all subtasks");
            });
    }

    [Fact]
    public async Task Applies_due_date_labels_to_all_subtasks()
    {
        // Arrange
        var taskId = 1;
        await _context.Map(
            ctx =>
            {
                var taxonomyEntity = ATaxonomy()
                    .WithId(1)
                    .WithName("Institution_Type_Access")
                    .WithDisplayName("Institution types")
                    .IsMandatory()
                    .IsMultiSelect()
                    .WithTags(ATag().WithId((int)SchoolType.Academy).WithName(nameof(SchoolType.Academy)))
                    .Build();
                var subtaskSource = new SourceEntity { Id = (int)Source.Subtask, Name = nameof(Source.Subtask) };
                var taskEntity = ATask()
                    .WithId(taskId)
                    .WithSubTasks(
                        ASubTask()
                            .WithId(1)
                            .InState(Published)
                            .WithStartDate(2026, 5, 1)
                            .WithDueDate(2026, 6, 1, isExact: true),
                        ASubTask()
                            .WithId(2)
                            .InState(Expired)
                            .WithStartDate(2026, 3, 1)
                            .WithDueDate(2026, 3, 31, isExact: false)
                    )
                    .Build();

                ctx.Taxonomies.Add(taxonomyEntity);
                ctx.SchoolTypeTagMappings.AddRange(
                    taxonomyEntity.Tags.Select((x, i) => WithSchoolType(x, i + 1, (SchoolType)(i + 1)))
                );
                ctx.Tasks.Add(taskEntity);
                ctx.TagsSourceMappings.AddRange(
                    new TagsSourceMappingEntity
                    {
                        Id = 1,
                        Tag = taxonomyEntity.Tags.ElementAt(0),
                        TagId = taxonomyEntity.Tags.ElementAt(0).Id,
                        SubTask = taskEntity.SubTasks.ElementAt(0),
                        EntityId = taskEntity.SubTasks.ElementAt(0).Id,
                        Source = subtaskSource,
                        SourceId = subtaskSource.Id,
                    },
                    new TagsSourceMappingEntity
                    {
                        Id = 2,
                        Tag = taxonomyEntity.Tags.ElementAt(0),
                        TagId = taxonomyEntity.Tags.ElementAt(0).Id,
                        SubTask = taskEntity.SubTasks.ElementAt(1),
                        EntityId = taskEntity.SubTasks.ElementAt(1).Id,
                        Source = subtaskSource,
                        SourceId = subtaskSource.Id,
                    }
                );

                ctx.Tasks.Add(taskEntity);
            },
            CancellationToken.None
        );

        var query = new GetTaskByIdQuery(taskId);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result
            .Value.SubTasks.Should()
            .HaveCount(2)
            .And.AllSatisfy(st =>
            {
                st.DueDateLabel.Should()
                    .NotBeNullOrEmpty("due date labels should be applied to all subtasks with due dates");
            });
    }

    [Fact]
    public async Task Ensures_only_visible_related_tasks_are_present()
    {
        // Arrange
        await _context.Map(
            ctx =>
            {
                var taxonomyEntity = ATaxonomy()
                    .WithId(1)
                    .WithName("Institution_Type_Access")
                    .WithDisplayName("Institution types")
                    .IsMandatory()
                    .IsMultiSelect()
                    .WithTags(ATag().WithId((int)SchoolType.Academy).WithName(nameof(SchoolType.Academy)))
                    .Build();
                var taskEntity = ATask()
                    .WithId(1)
                    .WithSubTasks(
                        ASubTask().WithId(1).InState(Published).WithStartDate(2026, 5, 1).WithDueDate(2026, 6, 1)
                    )
                    .WithRelatedTask(
                        ATask().WithId(2).InState(Published).Named("Related Task 1 - Published"),
                        ATask().WithId(3).InState(Expired).Named("Related Task 2 - Expired"),
                        ATask().WithId(4).InState(Archived).Named("Related Task 3 - Archived")
                    )
                    .Build();
                var subtaskSource = new SourceEntity { Id = (int)Source.Subtask, Name = nameof(Source.Subtask) };

                ctx.Taxonomies.Add(taxonomyEntity);
                ctx.SchoolTypeTagMappings.AddRange(
                    taxonomyEntity.Tags.Select((x, i) => WithSchoolType(x, i + 1, (SchoolType)(i + 1)))
                );
                ctx.Tasks.Add(taskEntity);
                ctx.TagsSourceMappings.AddRange(
                    new TagsSourceMappingEntity
                    {
                        Id = 1,
                        Tag = taxonomyEntity.Tags.ElementAt(0),
                        TagId = taxonomyEntity.Tags.ElementAt(0).Id,
                        SubTask = taskEntity.SubTasks.ElementAt(0),
                        EntityId = taskEntity.SubTasks.ElementAt(0).Id,
                        Source = subtaskSource,
                        SourceId = subtaskSource.Id,
                    }
                );
            },
            CancellationToken.None
        );

        var query = new GetTaskByIdQuery(1);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result
            .Value.RelatedTasks.Should()
            .HaveCount(2)
            .And.ContainEquivalentOf(new { Name = "Related Task 1 - Published" })
            .And.ContainEquivalentOf(new { Name = "Related Task 2 - Expired" })
            .And.NotContainEquivalentOf(new { Name = "Related Task 3 - Archived" });
    }
}
