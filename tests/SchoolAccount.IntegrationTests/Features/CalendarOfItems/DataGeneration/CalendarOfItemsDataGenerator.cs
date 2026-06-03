using Bogus;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.IntegrationTests.Features.CalendarOfItems.DataGeneration;

public class CalendarOfItemsDataGenerator
{
    private readonly Faker _faker = new("en_GB") { Random = new Randomizer(123) };

    public ICollection<CalendarOfItemsRow> GenerateCalendarOfItemsRows(
        GenericQueryCriteria filter,
        int numberOfItems,
        int daysForward = 31,
        int daysBack = 180
    )
    {
        var filterDate = filter.Range;
        var isForward = filter.ViewModes == CalendarOfItemsViewModes.Forward;

        var listOfRows = new List<CalendarOfItemsRow>();

        for (var i = 1; i <= numberOfItems; i++)
        {
            var start = GenerateStartDate(daysForward, daysBack, isForward, filterDate);
            var due = GenerateDueDate(start, filterDate);

            var item = new CalendarOfItemsRow
            {
                Id = i,
                Name = _faker.Name.FullName(),
                Description = _faker.Lorem.Paragraph(),
                Type = CalendarOfItemsRowType.Task,
                LastUpdated = DateTime.Now.AddDays(-1).AddMonths(-1),
                StartDate = start,
                DueDate = due,
            };
            listOfRows.Add(item);
        }

        return listOfRows;
    }

    private DateOnly? GenerateDueDate(DateOnly? start, DateOnlyRange? filterDate)
    {
        DateOnly? due = null;
        if (start is null || _faker.Random.Bool())
        {
            due = _faker.Date.SoonDateOnly(31, start ?? filterDate?.End);
        }

        return due;
    }

    private DateOnly? GenerateStartDate(int daysForward, int daysBack, bool isForward, DateOnlyRange? filterDate)
    {
        DateOnly? start = null;
        if (_faker.Random.Bool())
        {
            start = isForward
                ? _faker.Date.SoonDateOnly(daysForward, filterDate?.Start)
                : _faker.Date.RecentDateOnly(daysBack, filterDate?.Start);
        }

        return start;
    }
}
