using System.Data.SqlTypes;
using FluentValidation;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Infrastructure.Resolvers;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;

namespace SchoolAccount.Infrastructure.Aggregators;

public class CalendarOfItemsAggregatorValidator : AbstractValidator<CalendarOfItemsCriteria>
{
    public CalendarOfItemsAggregatorValidator(CalendarOfItemsQueryFactoryResolver queryFactoryResolver)
    {
        RuleFor(x => x.ToQuery)
            .Must(q => q > CalendarOfItemsQueryTypes.None)
            .WithName(nameof(CalendarOfItemsCriteria.ToQuery))
            .WithMessage(x => $"{nameof(x.ToQuery)} is out of range.")
            .WithErrorCode(nameof(ArgumentOutOfRangeException));

        RuleFor(x => x.ViewModes)
            .Must(v => v > CalendarOfItemsViewModes.None)
            .WithName(nameof(CalendarOfItemsCriteria.ViewModes))
            .WithMessage(x => $"{nameof(x.ViewModes)} is out of range.")
            .WithErrorCode(nameof(ArgumentOutOfRangeException));

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage(x => $"{nameof(x.PageSize)} must be at least 1.")
            .WithErrorCode(nameof(ArgumentOutOfRangeException));

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage(x => $"{nameof(x.PageNumber)} must be at least 1.")
            .WithErrorCode(nameof(ArgumentOutOfRangeException));

        RuleFor(x => x.Range)
            .Must(x => x != default)
            .WithMessage(x => $"{nameof(x.Range)} has not been set.")
            .WithErrorCode(nameof(ArgumentException));

        RuleFor(x => x.Range.Start)
            .Must(min => min.ToDateTime(TimeOnly.MinValue) >= SqlDateTime.MinValue.Value)
            .WithMessage(x => $"{nameof(x.Range.Start)} is out of range.")
            .WithErrorCode(nameof(ArgumentOutOfRangeException));

        RuleFor(x => x.Range.End)
            .Must(max => max.ToDateTime(TimeOnly.MinValue) <= SqlDateTime.MaxValue.Value)
            .WithMessage(x => $"{nameof(x.Range.End)} is out of range.")
            .WithErrorCode(nameof(ArgumentOutOfRangeException));

        RuleFor(x => x.ToQuery)
            .Must(queryFactoryResolver.IsThereADefinedFactory)
            .WithMessage(x => $"One of, {x.ToQuery}, types do not have a query factory implemented.")
            .WithErrorCode(nameof(NotImplementedException));
    }
}
