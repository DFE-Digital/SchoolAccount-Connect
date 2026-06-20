using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace SchoolAccount.Web.Connect.Features.CalendarOfItems;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public sealed class CalendarOfItemsRequestValidator : AbstractValidator<CalendarOfItemsRequest>
{
    public CalendarOfItemsRequestValidator()
    {
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);

        RuleFor(x => x.ViewModes).IsInEnum();

        RuleFor(x => x.SortMode).IsInEnum();
    }
}
