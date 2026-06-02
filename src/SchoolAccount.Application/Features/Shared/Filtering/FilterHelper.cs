using System.Collections.ObjectModel;
using System.Linq.Expressions;
using SchoolAccount.Application.Features.Shared.Filtering.Models;

namespace SchoolAccount.Application.Features.Shared.Filtering;

public static class FilterHelper
{
    internal static Expression Condition(Expression property, FilterRequest node)
    {
        return node.Operator switch
        {
            ComparisonType.Equals => ExpressionBuilders.Equals(property, property.Type, node.Value),
            ComparisonType.NotEquals => Expression.Not(ExpressionBuilders.Equals(property, property.Type, node.Value)),

            ComparisonType.LessThan => ExpressionBuilders.Compare(ExpressionType.LessThan, property, node.Value),
            ComparisonType.LessThanOrEquals => ExpressionBuilders.Compare(
                ExpressionType.LessThanOrEqual,
                property,
                node.Value
            ),
            ComparisonType.GreaterThan => ExpressionBuilders.Compare(ExpressionType.GreaterThan, property, node.Value),
            ComparisonType.GreaterThanOrEquals => ExpressionBuilders.Compare(
                ExpressionType.GreaterThanOrEqual,
                property,
                node.Value
            ),

            ComparisonType.In => ExpressionBuilders.In(property, property.Type, node.Value),
            ComparisonType.NotIn => Expression.Not(ExpressionBuilders.In(property, property.Type, node.Value)),

            ComparisonType.Contains => ExpressionBuilders.Contains(property, property.Type, node.Value),
            ComparisonType.NotContains => Expression.Not(
                ExpressionBuilders.Contains(property, property.Type, node.Value)
            ),

            _ => throw new InvalidOperationException("Bad operator"),
        };
    }

    internal static Expression Build<T>(ParameterExpression param, FilterRequest node, FieldSelectorMapping map)
    {
        if (node.Children.Count > 0)
        {
            var children = node.Children.Select(x => Build<T>(param, x, map)).ToList();

            return node.Join == JoinType.Or
                ? children.Aggregate(Expression.OrElse)
                : children.Aggregate(Expression.AndAlso);
        }

        if (!map[typeof(T)].TryGetValue(node.Field!, out var selector))
        {
            throw new InvalidOperationException($"Invalid field {node.Field}");
        }

        var property = new ParameterReplaceVisitor(selector.Parameters[0], param).Visit(selector.Body)!;

        return Condition(property, node);
    }

    public static IQueryable<T> Apply<T>(
        this IQueryable<T> query,
        IList<FilterRequest> filters,
        FieldSelectorMapping map
    )
    {
        foreach (var filter in filters)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var body = Build<T>(param, filter, map);
            var lambda = Expression.Lambda<Func<T, bool>>(body, param);
            query = query.Where(lambda);
        }

        return query;
    }
}
