using System.Globalization;
using AwesomeAssertions;
using SchoolAccount.Application.Common;
using Xunit;

namespace SchoolAccount.Application.UnitTests.Common;

public class DueDateLabelTests
{
    [Fact]
    public void Returns_empty_string_when_due_date_is_missing()
    {
        // Arrange
        DateOnly? dueDate = null;
        bool? isExactDate = true;

        // Act
        var result = DueDateLabel.Generate(dueDate, isExactDate);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Fact]
    public void Returns_empty_string_when_exact_flag_is_missing()
    {
        // Arrange
        var dueDate = new DateOnly(2026, 3, 15);
        bool? isExactDate = null;

        // Act
        var result = DueDateLabel.Generate(dueDate, isExactDate);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Fact]
    public void Returns_empty_string_when_both_due_date_and_exact_flag_are_missing()
    {
        // Arrange
        DateOnly? dueDate = null;
        bool? isExactDate = null;

        // Act
        var result = DueDateLabel.Generate(dueDate, isExactDate);

        // Assert
        result.Should().Be(string.Empty);
    }

    [Fact]
    public void Shows_full_date_with_day_when_date_is_exact()
    {
        // Arrange
        var dueDate = new DateOnly(2026, 3, 15);
        bool? isExactDate = true;

        // Act
        var result = DueDateLabel.Generate(dueDate, isExactDate);

        // Assert
        result.Should().Be("Due 15 Mar 2026.");
    }

    [Fact]
    public void Shows_month_and_year_only_when_date_is_approximate()
    {
        // Arrange
        var dueDate = new DateOnly(2026, 3, 15);
        bool? isExactDate = false;

        // Act
        var result = DueDateLabel.Generate(dueDate, isExactDate);

        // Assert
        result.Should().Be("Due Mar 2026.");
    }

    [Theory]
    [InlineData(2026, 1, 1, "Due 1 Jan 2026.")]
    [InlineData(2026, 12, 31, "Due 31 Dec 2026.")]
    [InlineData(2026, 6, 15, "Due 15 Jun 2026.")]
    [InlineData(2025, 2, 28, "Due 28 Feb 2025.")]
    public void Formats_exact_dates_with_day_month_and_year(int year, int month, int day, string expected)
    {
        // Arrange
        var dueDate = new DateOnly(year, month, day);
        bool? isExactDate = true;

        // Act
        var result = DueDateLabel.Generate(dueDate, isExactDate);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(2026, 1, 1, "Due Jan 2026.")]
    [InlineData(2026, 12, 31, "Due Dec 2026.")]
    [InlineData(2026, 6, 15, "Due Jun 2026.")]
    [InlineData(2025, 2, 28, "Due Feb 2025.")]
    public void Formats_approximate_dates_with_month_and_year_only(int year, int month, int day, string expected)
    {
        // Arrange
        var dueDate = new DateOnly(year, month, day);
        bool? isExactDate = false;

        // Act
        var result = DueDateLabel.Generate(dueDate, isExactDate);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(true, "Due 15 Mar 2026.")]
    [InlineData(false, "Due Mar 2026.")]
    public void Adjusts_date_format_based_on_exact_flag(bool isExact, string expected)
    {
        // Arrange
        var dueDate = new DateOnly(2026, 3, 15);

        // Act
        var result = DueDateLabel.Generate(dueDate, isExact);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("en-GB", 2026, 3, 15, true, "Due 15 Mar 2026.", "English exact date with day and abbreviated month")]
    [InlineData("cy-GB", 2026, 3, 15, true, "Due 15 Maw 2026.", "Welsh exact date with day and abbreviated month")]
    [InlineData("en-GB", 2026, 3, 15, false, "Due Mar 2026.", "English approximate date with abbreviated month only")]
    [InlineData("cy-GB", 2026, 3, 15, false, "Due Maw 2026.", "Welsh approximate date with abbreviated month only")]
    public void Formats_dates_with_English_and_Welsh_month_names(
        string cultureName,
        int year,
        int month,
        int day,
        bool isExact,
        string expected,
        string reason
    )
    {
        // Arrange
        var culture = new CultureInfo(cultureName);
        var previousCulture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = culture;

        try
        {
            var dueDate = new DateOnly(year, month, day);

            // Act
            var result = DueDateLabel.Generate(dueDate, isExact);

            // Assert
            result.Should().Be(expected, because: reason);
        }
        finally
        {
            CultureInfo.CurrentCulture = previousCulture;
        }
    }
}
