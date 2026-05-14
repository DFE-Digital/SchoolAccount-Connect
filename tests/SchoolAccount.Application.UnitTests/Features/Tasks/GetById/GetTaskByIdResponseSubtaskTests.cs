using AwesomeAssertions;
using SchoolAccount.Application.Features.Tasks.GetById;
using SchoolAccount.Domain.Common;
using Xunit;

namespace SchoolAccount.Application.UnitTests.Features.Tasks.GetById;

public static class GetTaskByIdResponseSubtaskTests
{
    public class IsOptional
    {
        [Fact]
        public void Returns_true_when_requirement_is_optional()
        {
            // Arrange
            var sut = new GetTaskByIdResponseSubtask { Requirement = Requirement.Optional };

            // Act & Assert
            sut.IsOptional.Should().BeTrue();
        }

        [Fact]
        public void Returns_false_when_requirement_is_mandatory()
        {
            // Arrange
            var sut = new GetTaskByIdResponseSubtask { Requirement = Requirement.Mandatory };

            // Act & Assert
            sut.IsOptional.Should().BeFalse();
        }
    }

    public class SortingDate
    {
        [Fact]
        public void Prioritises_due_date_when_available()
        {
            // Arrange
            var subtask = new GetTaskByIdResponseSubtask
            {
                StartDate = new DateOnly(2026, 3, 1),
                DueDate = new DateOnly(2026, 3, 15),
            };

            // Act
            var result = subtask.SortingDate;

            // Assert
            result.Should().Be(new DateOnly(2026, 3, 15));
        }

        [Fact]
        public void Falls_back_to_start_date_when_due_date_unavailable()
        {
            // Arrange
            var subtask = new GetTaskByIdResponseSubtask { StartDate = new DateOnly(2026, 3, 1) };

            // Act
            var result = subtask.SortingDate;

            // Assert
            result.Should().Be(new DateOnly(2026, 3, 1));
        }

        [Fact]
        public void Returns_null_when_no_dates_exist()
        {
            // Arrange
            var subtask = new GetTaskByIdResponseSubtask();

            // Act
            var result = subtask.SortingDate;

            // Assert
            result.Should().BeNull();
        }
    }
}
