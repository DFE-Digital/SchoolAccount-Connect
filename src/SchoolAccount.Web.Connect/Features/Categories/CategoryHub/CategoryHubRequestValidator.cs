using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace SchoolAccount.Web.Connect.Features.Categories.CategoryHub;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public class CategoryHubRequestValidator : AbstractValidator<CategoryHubRequest>
{
    public CategoryHubRequestValidator()
    {
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
    }
}
