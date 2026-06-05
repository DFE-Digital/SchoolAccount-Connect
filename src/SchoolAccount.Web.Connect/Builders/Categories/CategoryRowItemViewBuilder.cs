using System.Globalization;
using SchoolAccount.Application.Features.Categories.Models;
using SchoolAccount.Web.Connect.Models.Categories;

namespace SchoolAccount.Web.Connect.Builders.Categories;

public class CategoryRowItemViewBuilder
{
    private static string DetermineUri(long? id)
    {
        return id.HasValue
            ? string.Format(CultureInfo.InvariantCulture, RouteConstants.Category.Hub, id.Value)
            : RouteConstants.Task.AllTasks;
    }

    public CategoryListRowItemViewModel Build(CategoryRow row)
    {
        return new CategoryListRowItemViewModel(row.Name, DetermineUri(row.Id)) { Description = row.Description };
    }
}
