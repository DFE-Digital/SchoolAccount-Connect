using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using static SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums.CalendarOfItemsViewModes;

namespace SchoolAccount.Web.Connect.Features.Calendars.CalendarOfItems;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public sealed class CalendarOfItemsRequestValidator : AbstractValidator<CalendarOfItemsRequest>
{
    public CalendarOfItemsRequestValidator()
    {
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);

        RuleFor(x => x.ViewModes).IsInEnum();

        RuleFor(x => x.ViewModes)
            .Must(viewModes => !viewModes.HasFlag(Forward | Backward))
            .WithMessage("ViewModes cannot have both Forward and Backward set.");

        RuleFor(x => x.SortMode).IsInEnum();
    }
}
