using MockQueryable.NSubstitute;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Features.Tasks.GetById;
using SchoolAccount.Domain.Tasks;
using SchoolAccount.Kernel;
using static SchoolAccount.Domain.Common.WorkflowState;
using static SchoolAccount.Tests.Common.Builders.SubTaskBuilder;
using static SchoolAccount.Tests.Common.Builders.TaskBuilder;

namespace SchoolAccount.Application.IntegrationTests;

public class GetTaskByIdHandlerTests
{
    private readonly IApplicationDbContext _context;
    private readonly GetTaskByIdHandler _sut;

    public GetTaskByIdHandlerTests()
    {
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(new DateTime(2026, 4, 21, 10, 0, 0, DateTimeKind.Utc));

        _context = Substitute.For<IApplicationDbContext>();
        _sut = new GetTaskByIdHandler(_context, dateTimeProvider);
    }

    [Fact]
    public async Task Returns_error_response_when_task_not_found()
    {
        // Arrange
        var tasks = Array.Empty<TaskEntity>().BuildMockDbSet();
        var expectedError = GetTaskByIdErrors.NotFound(999);
        var query = new GetTaskByIdQuery(999);

        _context.Tasks.Returns(tasks);

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
        var taskEntity = ATask()
            .WithId(1)
            .WithSubTasks(
                ASubTask().WithId(1).InState(Published).WithStartDate(2026, 5, 1).WithDueDate(2026, 6, 1),
                ASubTask().WithId(2).InState(Expired).WithStartDate(2026, 3, 1).WithDueDate(2026, 3, 31)
            )
            .Build();

        var tasks = new[] { taskEntity }.BuildMockDbSet();
        _context.Tasks.Returns(tasks);

        var query = new GetTaskByIdQuery(1);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
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
        var taskEntity = ATask()
            .WithId(1)
            .WithSubTasks(
                ASubTask()
                    .WithId(1)
                    .InState(Published)
                    .WithStartDate(2026, 5, 1)
                    .WithDueDate(2026, 6, 1, isExact: true),
                ASubTask().WithId(2).InState(Expired).WithStartDate(2026, 3, 1).WithDueDate(2026, 3, 31, isExact: false)
            )
            .Build();

        var tasks = new[] { taskEntity }.BuildMockDbSet();
        _context.Tasks.Returns(tasks);

        var query = new GetTaskByIdQuery(1);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result
            .Value.SubTasks.Should()
            .HaveCount(2)
            .And.AllSatisfy(st =>
            {
                st.DueDateLabel.Should()
                    .NotBeNullOrEmpty("due date labels should be applied to all subtasks with due dates");
            });
    }
}
