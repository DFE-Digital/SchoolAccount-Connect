using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Categories.GetCategoryHub;

public static class GetCategoryHubErrors
{
    public static Error NotFound(int categoryId) =>
        Error.NotFound("Category.NotFound", $"The category with the Id = '{categoryId}' was not found");
}
