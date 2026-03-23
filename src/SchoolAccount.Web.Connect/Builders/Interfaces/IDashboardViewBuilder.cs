using SchoolAccount.Web.Connect.Models;

namespace SchoolAccount.Web.Connect.Builders.Interfaces;

public interface IDashboardViewBuilder
{
    Task<DashboardViewModel> Build(CancellationToken cancellationToken);
}
