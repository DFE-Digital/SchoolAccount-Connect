using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Factories.Query;

[SuppressMessage("Usage", "CA2234:Pass system uri objects instead of strings")]
public class QueryFactoryForJiraIntegrationsForCalendarOfItems(IHttpClientFactory clientFactory)
    : IQueryFactory<CalendarOfItemsRow>
{
    public Type? TypeBeingRegistered => typeof(CanonicalTask);

    public async Task<QueryResponse<CalendarOfItemsRow>> Query(GenericQueryCriteria<CalendarOfItemsRow> criteria, FieldSelectorMapping mappings,
        CancellationToken cancellationToken)
    {
        using var client = clientFactory.CreateClient();
        using var response = await client.GetAsync("http://127.0.0.1:5000/integrations/jira/tasks", cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var data = (await response.Content.ReadFromJsonAsync<IReadOnlyList<CanonicalTask>>(cancellationToken) ?? [])
            .Where(ct => ct.DueDate.HasValue)
            .ToList();

        return (
            data.Count,
            data
                .Select(ct => new CalendarOfItemsRow
                {
                    Id = ct.Id,
                    Name = ct.Title,
                    Description = ct.Description,
                    Source = "Jira",
                    SortDate = ToDateOnly(ct.DueDate!.Value),
                    Type = CalendarOfItemsRowType.Task,
                }));
    }

    private static DateOnly ToDateOnly(DateTimeOffset dt) => new(dt.Year, dt.Month, dt.Day);
}