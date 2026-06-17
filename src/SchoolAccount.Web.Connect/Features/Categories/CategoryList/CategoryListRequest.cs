namespace SchoolAccount.Web.Connect.Features.Categories.CategoryList;

public class CategoryListRequest
{
    public int PageSize { get; init; } = 10;

    public int PageNumber { get; init; } = 1;
}
