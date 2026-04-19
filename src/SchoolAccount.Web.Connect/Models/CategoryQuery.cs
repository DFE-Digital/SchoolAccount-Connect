using SchoolAccount.Application.Features.Category.Enums;

namespace SchoolAccount.Web.Connect.Models;

public class CategoryQuery
{
    public CategoryListViewModes ViewModes { get; init; } = CategoryListViewModes.None;

    public int PageSize { get; init; } = 10;

    public int PageNumber { get; init; } = 1;
}
