using System.Globalization;
using AwesomeAssertions;
using NSubstitute.ExceptionExtensions;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Interfaces;
using SchoolAccount.Application.Features.CalendarOfItems.GetCalendarOfItemsOfSubTasksByDirectionForTabView;
using SchoolAccount.Kernel;
using Xunit;

namespace SchoolAccount.Application.UnitTests;

public class CalendarOfItemsDetermineDateRangeTests
{
    [Theory]
    [InlineData(1, "11/03/2026", "01/02/2026", "31/03/2026")]
    [InlineData(1, "01/03/2026", "01/02/2026", "31/03/2026")]
    [InlineData(1, "11/02/2026", "01/01/2026", "28/02/2026")]
    [InlineData(1, "01/01/2026", "01/12/2025", "31/01/2026")]
    [InlineData(2, "02/01/2026", "01/11/2025", "31/01/2026")]
    [InlineData(3, "13/02/2026", "01/11/2025", "28/02/2026")]
    public async Task Ensure_DetermineDateRange_Generates_Range_Correctly_When_Going_Backwards(
        int monthPeriod,
        string from,
        string expectedStart,
        string expectedEnd
    )
    {
        // Arrange
        var expectedStartDate = DateOnly.FromDateTime(
            DateTime.ParseExact(expectedStart, "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        var expectedEndDate = DateOnly.FromDateTime(
            DateTime.ParseExact(expectedEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        var expectedResult = new DateOnlyRange(expectedStartDate, expectedEndDate);
        var fromDate = DateOnly.FromDateTime(DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture));

        var calendarOfItemsDirectionalQuery = new DetermineDateRangeTestCalendarOfItemQuery(
            CalendarOfItemsViewModes.Backward,
            monthPeriod,
            fromDate
        );

        // Act
        var result = calendarOfItemsDirectionalQuery.DetermineDateRange();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Theory]
    [InlineData(1, "11/03/2026", "01/03/2026", "30/04/2026")]
    [InlineData(1, "01/03/2026", "01/03/2026", "30/04/2026")]
    [InlineData(1, "21/03/2026", "01/03/2026", "30/04/2026")]
    [InlineData(1, "11/02/2026", "01/02/2026", "31/03/2026")]
    [InlineData(2, "01/01/2026", "01/01/2026", "31/03/2026")]
    [InlineData(3, "21/04/2026", "01/04/2026", "31/07/2026")]
    public async Task Ensure_DetermineDateRange_Generates_Range_Correctly_When_Going_Forwards(
        int monthPeriod,
        string from,
        string expectedStart,
        string expectedEnd
    )
    {
        // Arrange
        var expectedStartDate = DateOnly.FromDateTime(
            DateTime.ParseExact(expectedStart, "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        var expectedEndDate = DateOnly.FromDateTime(
            DateTime.ParseExact(expectedEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );
        var expectedResult = new DateOnlyRange(expectedStartDate, expectedEndDate);
        var fromDate = DateOnly.FromDateTime(DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture));

        var calendarOfItemsDirectionalQuery = new DetermineDateRangeTestCalendarOfItemQuery(
            CalendarOfItemsViewModes.Forward,
            monthPeriod,
            fromDate
        );

        // Act
        var result =
            calendarOfItemsDirectionalQuery.DetermineDateRange();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Theory]
    [InlineData(CalendarOfItemsViewModes.Standalone)]
    [InlineData(CalendarOfItemsViewModes.Custom)]
    [InlineData(CalendarOfItemsViewModes.Forward | CalendarOfItemsViewModes.Backward)]
    public void Ensure_that_DetermineDateRange_throws_exception_if_query_invalid(CalendarOfItemsViewModes calendarOfItemsViewModes)
    {
        // Arrange
        var fromDate = DateOnly.FromDateTime(
            DateTime.ParseExact("11/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );

        var calendarOfItemsDirectionalQuery = new DetermineDateRangeTestCalendarOfItemQuery(
            calendarOfItemsViewModes,
            1,
            fromDate
        );

        // Act
        Action act = () => calendarOfItemsDirectionalQuery.DetermineDateRange();

        // Assert
        act.Should().Throw<Exception>()
            .Which.Should()
            .Match<Exception>(ex => ex is InvalidOperationException || ex is ArgumentOutOfRangeException);
    }

    private sealed record DetermineDateRangeTestCalendarOfItemQuery
        : GetCalendarOfItemsOfSubTasksByDirectionForTabViewQuery
    {
        public DetermineDateRangeTestCalendarOfItemQuery(
            CalendarOfItemsViewModes viewModes,
            int viewPeriod,
            DateOnly date
        ) : base(
            viewModes,
            viewPeriod,
            date: date
        )
        {
        }
    }
}
