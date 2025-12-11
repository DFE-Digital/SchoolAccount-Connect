
using SchoolAccount.Application.Features.Shared;

namespace SchoolAccount.Application.Features.Dashboard.Queries.GetDashboard;

public class RecentPageEditItem : BaseListItem
{
    
}

public class RecentPagePublishedItem : BaseListItem
{
    
}

public record DashboardLists(IReadOnlyCollection<RecentPageEditItem> PageEdits,
    IReadOnlyCollection<RecentPagePublishedItem> PagePublished);