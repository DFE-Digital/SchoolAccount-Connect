using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace SchoolAccount.Web.Connect.Features.Calendar.CalendarList;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public sealed class CalendarListRequestValidator : AbstractValidator<CalendarListRequest>
{
    public CalendarListRequestValidator()
    {
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);

        RuleFor(x => x.ViewModes).IsInEnum();

        RuleFor(x => x.SortMode).IsInEnum();
    }
}
