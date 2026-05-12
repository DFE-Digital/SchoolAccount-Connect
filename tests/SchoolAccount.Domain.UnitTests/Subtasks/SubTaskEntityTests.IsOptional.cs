using AwesomeAssertions;
using SchoolAccount.Domain.Common;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.SubTaskBuilder;

// ReSharper disable ClassNeverInstantiated.Global

namespace SchoolAccount.Domain.UnitTests.Subtasks;

public sealed partial class SubTaskEntityTests
{
    public sealed class IsOptional
    {
        [Fact]
        public void Returns_true_when_requirement_is_optional()
        {
            // Arrange
            var subTask = ASubTask().WithRequirement(Requirement.Optional).Build();

            // Act
            var result = subTask.IsOptional;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_false_when_requirement_is_mandatory()
        {
            // Arrange
            var subTask = ASubTask().WithRequirement(Requirement.Mandatory).Build();

            // Act
            var result = subTask.IsOptional;

            // Assert
            result.Should().BeFalse();
        }
    }
}
