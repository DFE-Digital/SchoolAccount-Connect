using SchoolAccount.Application.Features.Category.Models;
using SchoolAccount.Web.Connect.Models.Categories;

namespace SchoolAccount.Web.Connect.Builders.Categories;

public class CategoryRowItemViewBuilder
{
    private static string DetermineUri(long? id)
    {
        return id.HasValue 
            ? $"{RouteConstants.Root}categories/{id}" 
            : RouteConstants.Category.AllTasks;
    }

    public CategoryListRowItemViewModel Build(CategoryRow row)
    {
        return new CategoryListRowItemViewModel(row.Name, DetermineUri(row.Id)) { Description = row.Description };
    }
}
