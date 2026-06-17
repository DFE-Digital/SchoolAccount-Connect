using SchoolAccount.Application.Pipelines.Filters;
using SchoolAccount.Application.Pipelines.Query;

namespace SchoolAccount.Application.Pipelines;

public class CalendarOfItemsPipeline(
    CalendarOfItemsQueryPipeline query,
    CalendarOfItemsFilterPipeline filters
)
{
    public CalendarOfItemsQueryPipeline Query => query;
    public CalendarOfItemsFilterPipeline Filters => filters;   
}