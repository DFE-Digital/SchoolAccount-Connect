using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace SchoolAccount.Web.Connect.Features.Tasks.Search;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public sealed class SearchTasksRequestValidator : AbstractValidator<SearchTasksRequest>
{
    public SearchTasksRequestValidator()
    {
        RuleFor(x => x.Term).NotEmpty();

        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
    }
}
