using System.Globalization;
using AwesomeAssertions;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Query.Operational;
using SchoolAccount.Kernel;
using Xunit;

namespace SchoolAccount.Application.UnitTests;

public class DetermineDateRangeTests
{
    [Theory]
    [InlineData(1, "11/03/2026", "01/02/2026", "31/03/2026")]
    [InlineData(1, "01/03/2026", "01/02/2026", "31/03/2026")]
    [InlineData(1, "11/02/2026", "01/01/2026", "28/02/2026")]
    [InlineData(1, "01/01/2026", "01/12/2025", "31/01/2026")]
    [InlineData(2, "02/01/2026", "01/11/2025", "31/01/2026")]
    [InlineData(3, "13/02/2026", "01/11/2025", "28/02/2026")]
    public async Task CheckDetermineDateRangeFiltersCorrectDateOnlyRangeWhenBackwardViewMode(
        int monthPeriod,
        string from,
        string expectedStart,
        string expectedEnd
    )
    {
        var expectedStartDate = DateOnly.FromDateTime(
            DateTime.ParseExact(expectedStart, "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );

        var expectedEndDate = DateOnly.FromDateTime(
            DateTime.ParseExact(expectedEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );

        var fromDate = DateOnly.FromDateTime(DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture));

        var calendarOfItemsDirectionalQuery = new DetermineDateRangeTestCalendarOfItemQuery(
            CalendarOfItemsViewModes.Backward,
            monthPeriod,
            fromDate
        );

        var result = CalendarOfItemsDirectionalQueryHandler.DetermineDateRange(calendarOfItemsDirectionalQuery);

        var expectedResult = new DateOnlyRange(expectedStartDate, expectedEndDate);

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Theory]
    [InlineData(1, "11/03/2026", "01/03/2026", "30/04/2026")]
    [InlineData(1, "01/03/2026", "01/03/2026", "30/04/2026")]
    [InlineData(1, "21/03/2026", "01/03/2026", "30/04/2026")]
    [InlineData(1, "11/02/2026", "01/02/2026", "31/03/2026")]
    [InlineData(2, "01/01/2026", "01/01/2026", "31/03/2026")]
    [InlineData(3, "21/04/2026", "01/04/2026", "31/07/2026")]
    public Task CheckDetermineDateRangeFiltersCorrectDateOnlyRangeWhenForwardViewMode(
        int monthPeriod,
        string from,
        string expectedStart,
        string expectedEnd
    )
    {
        var expectedStartDate = DateOnly.FromDateTime(
            DateTime.ParseExact(expectedStart, "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );

        var expectedEndDate = DateOnly.FromDateTime(
            DateTime.ParseExact(expectedEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );

        var fromDate = DateOnly.FromDateTime(DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture));

        var calendarOfItemsDirectionalQuery = new DetermineDateRangeTestCalendarOfItemQuery(
            CalendarOfItemsViewModes.Forward,
            monthPeriod,
            fromDate
        );

        var result = CalendarOfItemsDirectionalQueryHandler.DetermineDateRange(calendarOfItemsDirectionalQuery);

        var expectedResult = new DateOnlyRange(expectedStartDate, expectedEndDate);

        result.Should().BeEquivalentTo(expectedResult);
        return Task.CompletedTask;
    }

    [Theory]
    [InlineData(CalendarOfItemsViewModes.Standalone)]
    [InlineData(CalendarOfItemsViewModes.None)]
    [InlineData(CalendarOfItemsViewModes.Custom)]
    public void CheckExceptionThrownWhenUnsupportedViewMode(CalendarOfItemsViewModes calendarOfItemsViewModes)
    {
        var expectedStartDate = DateOnly.FromDateTime(
            DateTime.ParseExact("01/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );

        var expectedEndDate = DateOnly.FromDateTime(
            DateTime.ParseExact("30/04/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );

        var fromDate = DateOnly.FromDateTime(
            DateTime.ParseExact("11/03/2026", "dd/MM/yyyy", CultureInfo.InvariantCulture)
        );

        var calendarOfItemsDirectionalQuery = new DetermineDateRangeTestCalendarOfItemQuery(
            calendarOfItemsViewModes,
            1,
            fromDate
        );

        Assert.Throws<InvalidOperationException>(() =>
            CalendarOfItemsDirectionalQueryHandler.DetermineDateRange(calendarOfItemsDirectionalQuery)
        );
    }

    private sealed record DetermineDateRangeTestCalendarOfItemQuery : CalendarOfItemsDirectionalQuery
    {
        public DetermineDateRangeTestCalendarOfItemQuery(
            CalendarOfItemsViewModes viewModes,
            int viewPeriod,
            DateOnly date
        )
            : base(
                CalendarOfItemsQueryTypes.None,
                viewModes,
                viewPeriod,
                date,
                1,
                1,
                CalendarOfItemsSortMode.NotSpecified
            ) { }
    }
}
