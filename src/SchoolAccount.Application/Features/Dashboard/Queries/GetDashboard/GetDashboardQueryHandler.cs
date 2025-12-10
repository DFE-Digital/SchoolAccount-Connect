using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Dashboard.Queries.GetDashboard;

public class GetDashboardQueryHandler()
    : IQueryHandler<GetDashboardQuery, DashboardLists>
{
    public async Task<Result<DashboardLists>> Handle(GetDashboardQuery query, CancellationToken cancellationToken)
    {
        var recentEdits = new DashboardLists([], []);
        return await Task.FromResult(Result.Success(recentEdits));
    }
}