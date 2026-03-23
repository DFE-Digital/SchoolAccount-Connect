using SchoolAccount.Web.Connect.Models;
using X.PagedList;

namespace SchoolAccount.Web.Connect.Builders.Interfaces;

public interface IPaginationViewBuilder
{
    PaginationViewModel Build(Pagination model, string endpoint, HttpRequest? request = null);
    PaginationViewModel Build<T>(T model) where T : IPagedList;
}
