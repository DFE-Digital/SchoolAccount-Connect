using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Features.Dashboard;

public sealed record GetDashboardQuery(DateOnly Date) : IQuery<GetDashboardResponse>;
