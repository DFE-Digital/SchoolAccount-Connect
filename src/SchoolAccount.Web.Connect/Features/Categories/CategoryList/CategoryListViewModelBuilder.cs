using System.Globalization;
using SchoolAccount.Application.Common;
using SchoolAccount.Application.Features.Categories.GetParentCategories;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Features.Shared.List;
using SchoolAccount.Web.Connect.Features.Shared.Pagination;
using X.PagedList;

namespace SchoolAccount.Web.Connect.Features.Categories.CategoryList;

public static class CategoryListViewModelBuilder
{
    public static CategoryListViewModel Build(GetParentCategoriesResponse response) =>
        new()
        {
            Categories = new PaginatedListViewModel(
                MapCategoriesToPagedList(response.Categories),
                "No tasks found for the category."
            ),
        };

    private static IPagedList<ListItemViewModel> MapCategoriesToPagedList(
        PagedResult<GetParentCategoriesResponseCategory> pagedCategories
    )
    {
        var categories = pagedCategories.Items.Select(ToListItem).ToList();

        categories = AddAllTasksLink(pagedCategories, categories);

        return categories.ToStaticPagedList(
            pagedCategories.PageNumber,
            pagedCategories.PageSize,
            pagedCategories.TotalCount
        );
    }

    private static List<ListItemViewModel> AddAllTasksLink(
        PagedResult<GetParentCategoriesResponseCategory> pagedCategories,
        List<ListItemViewModel> categories
    )
    {
        if (pagedCategories.TotalCount > 0)
        {
            var allTasks = new ListItemViewModel("All tasks", RouteConstants.Task.AllTasks);
            categories.Insert(0, allTasks);
        }

        return categories;
    }

    private static ListItemViewModel ToListItem(GetParentCategoriesResponseCategory category) =>
        new(
            category.DisplayName,
            string.Format(CultureInfo.InvariantCulture, RouteConstants.Category.Index, category.Id),
            description: category.Description
        );
}
