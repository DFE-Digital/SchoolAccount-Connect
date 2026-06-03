using System.Data.SqlTypes;
using FluentValidation;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;

namespace SchoolAccount.Application.Aggregators;

public class QueryAggregatorValidator<TRow> : AbstractValidator<GenericQueryCriteria<TRow>>
    where TRow : IQueryRow
{
    public QueryAggregatorValidator()
    {
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
    }
}
