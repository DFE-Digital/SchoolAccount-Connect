using System.Globalization;
using AwesomeAssertions;
using NSubstitute;
using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Kernel;
using Xunit;
using static SchoolAccount.Domain.Common.WorkflowState;

namespace SchoolAccount.Domain.Tests.Unit.Subtasks;

public class AvailabilityCalculatorTests
{
    private static readonly DateTime UtcNow = new(2026, 4, 10, 12, 0, 0, DateTimeKind.Utc);
    private static readonly DateOnly Today = DateOnly.FromDateTime(UtcNow);
    private readonly SubTaskAvailabilityLabel _sut;

    public AvailabilityCalculatorTests()
    {
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(UtcNow);
        _sut = new SubTaskAvailabilityLabel(dateTimeProvider);
    }

    [Theory]
    [ClassData(typeof(AvailabilityScenarioData))]
    public void Task_is_considered_available_now_under_various_published_conditions(
        WorkflowState state,
        int? startOffset,
        bool? exact,
        int? dueOffset,
        string expected,
        string reason
    )
    {
        // Arrange
        var start = startOffset.HasValue ? Today.AddDays(startOffset.Value) : (DateOnly?)null;
        var due = dueOffset.HasValue ? Today.AddDays(dueOffset.Value) : (DateOnly?)null;

        // Act
        var result = _sut.Generate(state, start, exact, due);

        // Assert
        result.Should().Be(expected, because: reason);
    }

    [Theory]
    [ClassData(typeof(AvailabilityBoundaryData))]
    public void Task_availability_is_calculated_correctly_across_day_and_month_boundaries(
        string mockNow,
        string startDateStr,
        bool exact,
        string expected,
        string reason
    )
    {
        // Arrange
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        var parsedNow = DateTime.Parse(mockNow, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        dateTimeProvider.UtcNow.Returns(parsedNow);

        var sut = new SubTaskAvailabilityLabel(dateTimeProvider);
        var startDate = DateOnly.Parse(startDateStr, CultureInfo.InvariantCulture);

        // Act
        var result = sut.Generate(Published, startDate, exact, null);

        // Assert
        result.Should().Be(expected, because: reason);
    }
}
