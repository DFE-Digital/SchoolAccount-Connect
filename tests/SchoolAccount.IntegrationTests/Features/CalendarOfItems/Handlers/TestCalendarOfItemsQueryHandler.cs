using System.Collections.ObjectModel;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Common;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Query;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Kernel;

namespace SchoolAccount.IntegrationTests.Features.CalendarOfItems.Handlers;

public class TestCalendarOfItemsQueryHandler : IQueryHandler<CalendarOfItemsQuery, CalendarOfItemsResponse>
{
    private readonly List<CalendarOfItemsRow> _rows = [];
    private int _pageSize = 10;

    public async Task<Result<CalendarOfItemsResponse>> Handle(
        CalendarOfItemsQuery query,
        CancellationToken cancellationToken
    )
    {
        var pageNumber = query.PageNumber;
        var paginatedRows = PaginateRows(pageNumber);

        var emptyFilter = new Collection<Filterable>();
        var emptyCriteria = new CalendarOfItemsCriteria();

        var result = new CalendarOfItemsResponse(emptyCriteria, paginatedRows, emptyFilter);

        return await Task.FromResult(result);
    }

    public TestCalendarOfItemsQueryHandler AddRow(CalendarOfItemsRow row)
    {
        _rows.Add(row);

        return this;
    }

    public TestCalendarOfItemsQueryHandler AddRows(IEnumerable<CalendarOfItemsRow> rows)
    {
        foreach (var row in rows)
        {
            AddRow(row);
        }

        return this;
    }

    public void Clear()
    {
        _rows.Clear();
    }

    public TestCalendarOfItemsQueryHandler SetPageSize(int pageSize)
    {
        _pageSize = pageSize;

        return this;
    }

    private PagedResult<CalendarOfItemsRow> PaginateRows(int pageNumber)
    {
        var totalCount = _rows.Count;
        var paginatedItems = _rows.Skip(_pageSize * (pageNumber - 1)).Take(_pageSize).ToList();

        return new PagedResult<CalendarOfItemsRow>
        {
            Items = paginatedItems,
            PageNumber = pageNumber,
            PageSize = _pageSize,
            TotalCount = totalCount,
        };
    }
}
