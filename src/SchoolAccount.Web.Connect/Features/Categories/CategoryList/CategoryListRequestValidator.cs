using FluentValidation;

namespace SchoolAccount.Web.Connect.Features.Categories.CategoryList;

public class CategoryListRequestValidator : AbstractValidator<CategoryListRequest>
{
    public CategoryListRequestValidator()
    {
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
    }
}
