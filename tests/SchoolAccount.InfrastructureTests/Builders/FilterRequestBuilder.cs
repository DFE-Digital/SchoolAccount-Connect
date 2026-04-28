using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Shared.Filtering;

namespace SchoolAccount.InfrastructureTests.Builders;

public sealed class FilterRequestBuilder
{
    private readonly string _field;
    private readonly List<FilterRequestBuilder> _children = [];
    private object? _value;
    private JoinType _joinType = JoinType.And;
    private ComparisonType _comparisonType = ComparisonType.Equals;

    internal FilterRequestBuilder(string field)
    {
        _field = field;
    }

    public FilterRequestBuilder And()
    {
        _joinType = JoinType.And;
        return this;
    }

    public FilterRequestBuilder Or()
    {
        _joinType = JoinType.Or;
        return this;
    }

    public FilterRequestBuilder WithOperator(ComparisonType comparisonType)
    {
        _comparisonType = comparisonType;
        return this;
    }

    public FilterRequestBuilder WithValue(object? value)
    {
        _value = value;
        return this;
    }

    public FilterRequestBuilder WithValues(params object?[] value)
    {
        _value = value.ToList();
        return this;
    }

    public FilterRequestBuilder WithChildren(IEnumerable<FilterRequestBuilder> items)
    {
        _children.AddRange(items);
        return this;
    }

    public FilterRequestBuilder WithChildren(params FilterRequestBuilder[] items)
    {
        _children.AddRange(items);
        return this;
    }

    public FilterRequest Build()
    {
        return new FilterRequest
        {
            Join = _joinType,
            Field = _field,
            Value = _value,
            Operator = _comparisonType,
            Children = _children.Select(x => x.Build()).ToCollection()
        };
    }

    public static implicit operator FilterRequest(FilterRequestBuilder builder)
    {
        return builder.Build();
    }
}