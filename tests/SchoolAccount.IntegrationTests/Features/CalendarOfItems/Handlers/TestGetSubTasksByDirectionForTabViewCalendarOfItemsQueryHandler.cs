using System.Collections.ObjectModel;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.CalendarOfItems.Query.Operational;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.IntegrationTests.Features.CalendarOfItems.Handlers;

public class TestGetSubTasksByDirectionForTabViewCalendarOfItemsQueryHandler
    : IQueryHandler<CalendarOfItemsDirectionalQuery, QueryPagedResult>
{
    private readonly List<CalendarOfItemsRow> _rows = [];
    private int _pageSize = 10;

    public async Task<Result<QueryPagedResult>> Handle(
        CalendarOfItemsDirectionalQuery query,
        CancellationToken cancellationToken
    )
    {
        var paginatedRows = _rows.ToStaticPagedList(1, _pageSize, _rows.Count);
        var emptyFilter = new Collection<Filterable>();
        var emptyCriteria = new GenericQueryCriteria();

        var result = new QueryPagedResult(emptyCriteria, paginatedRows, emptyFilter);

        return await Task.FromResult(result);
    }

    public TestGetSubTasksByDirectionForTabViewCalendarOfItemsQueryHandler AddRow(CalendarOfItemsRow row)
    {
        _rows.Add(row);

        return this;
    }

    public TestGetSubTasksByDirectionForTabViewCalendarOfItemsQueryHandler AddRows(IEnumerable<CalendarOfItemsRow> rows)
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

    public TestGetSubTasksByDirectionForTabViewCalendarOfItemsQueryHandler SetPageSize(int pageSize)
    {
        _pageSize = pageSize;

        return this;
    }
}
