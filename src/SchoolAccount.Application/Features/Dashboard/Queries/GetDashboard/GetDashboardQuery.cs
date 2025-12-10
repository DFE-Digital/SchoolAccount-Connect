using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Features.Dashboard.Queries.GetDashboard;

public sealed record GetDashboardQuery : IQuery<DashboardLists>;