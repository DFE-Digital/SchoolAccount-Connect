using System.Collections.ObjectModel;
using Bogus;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Models;
using SchoolAccount.InfrastructureTests.Extensions;
using SchoolAccount.Kernel;

namespace SchoolAccount.InfrastructureTests.Builders;

public class CalendarOfItemsRowCollectionBuilder
{
    private readonly Faker _faker = new("en_GB") { Random = new Randomizer(123) };
    private readonly List<CalendarOfItemsRowBuilder> _rows = [];
    private readonly DateOnlyRange _range;

    internal CalendarOfItemsRowCollectionBuilder(DateOnlyRange range)
    {
        _range = range;
    }

    public class CalendarOfItemsRowOptions
    {
        public string? Name { get; set; }
        public DateOnly? Date { get; set; }
        public CalendarOfItemsRowType? Type { get; set; }
    }

    public CalendarOfItemsRowCollectionBuilder Populate(
        int numberOfRowsToCreate,
        Action<CalendarOfItemsRowOptions>? configure = null,
        Func<int, bool>? where = null,
        int startId = 1
    )
    {
        configure ??= _ => { };
        for (var i = 0; i < numberOfRowsToCreate; i++)
        {
            var id = startId + i;
            var options = new CalendarOfItemsRowOptions();

            if (where is null || where(id))
            {
                configure(options);
            }

            Add(
                CalendarOfItemsRowExtensions.Create(
                    startId + i,
                    options.Name ?? _faker?.Name.FullName() ?? string.Empty,
                    options.Date ?? _faker?.Date.BetweenDateOnly(_range.Start, _range.End) ?? _range.Start,
                    options.Type ?? CalendarOfItemsRowType.None
                )
            );
        }

        return this;
    }

    public CalendarOfItemsRowCollectionBuilder Populate(
        int numberOfRowsToCreate,
        Func<int, Faker, DateOnlyRange, CalendarOfItemsRowBuilder> row
    )
    {
        for (var i = 0; i < numberOfRowsToCreate; i++)
        {
            Add(row(i, _faker, _range));
        }

        return this;
    }

    public CalendarOfItemsRowCollectionBuilder Add(CalendarOfItemsRowBuilder row)
    {
        _rows.Add(row);
        return this;
    }

    public CalendarOfItemsRowCollectionBuilder Add(Func<Faker, CalendarOfItemsRowBuilder> row)
    {
        _rows.Add(row(_faker));
        return this;
    }

    public CalendarOfItemsRowCollectionBuilder Add(IEnumerable<CalendarOfItemsRowBuilder> rows)
    {
        _rows.AddRange(rows);
        return this;
    }

    public CalendarOfItemsRowCollectionBuilder Add(params CalendarOfItemsRowBuilder[] rows)
    {
        _rows.AddRange(rows);
        return this;
    }

    public CalendarOfItemsRowCollectionBuilder Apply(
        Func<CalendarOfItemsRowBuilder, bool> predicate,
        Action<CalendarOfItemsRowBuilder> action
    )
    {
        foreach (var row in _rows.Where(predicate))
        {
            action(row);
        }

        return this;
    }

    public CalendarOfItemsRowCollectionBuilder Apply(
        Func<CalendarOfItemsRowBuilder, bool> predicate,
        Action<CalendarOfItemsRowBuilder, Faker> action
    )
    {
        foreach (var row in _rows.Where(predicate))
        {
            action(row, _faker);
        }

        return this;
    }

    public CalendarOfItemsRowCollectionBuilder Apply(Action<CalendarOfItemsRowBuilder, Faker> action)
    {
        return Apply(_ => true, action);
    }

    public CalendarOfItemsRowCollectionBuilder Apply(Action<CalendarOfItemsRowBuilder> action)
    {
        return Apply(_ => true, action);
    }

    public CalendarOfItemsRowCollectionBuilder Apply(int index, Action<CalendarOfItemsRowBuilder> action)
    {
        if (index >= 0 || index < _rows.Count)
        {
            action(_rows[index]);
        }

        return this;
    }

    public CalendarOfItemsRowCollectionBuilder Apply(int index, Action<CalendarOfItemsRowBuilder, Faker> action)
    {
        if (index >= 0 || index < _rows.Count)
        {
            action(_rows[index], _faker);
        }

        return this;
    }

    public Collection<CalendarOfItemsRow> Build()
    {
        return _rows.Select(x => x.Build()).ToCollection();
    }

    public static implicit operator Collection<CalendarOfItemsRow>(CalendarOfItemsRowCollectionBuilder builder)
    {
        return builder.Build();
    }
}
