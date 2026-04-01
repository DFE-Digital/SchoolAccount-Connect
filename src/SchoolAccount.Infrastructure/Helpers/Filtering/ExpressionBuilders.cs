using System.Collections;
using System.Globalization;
using System.Linq.Expressions;

namespace SchoolAccount.Infrastructure.Helpers.Filtering;

public static class ExpressionBuilders
{
    public static Expression Equals(Expression property, Type type, object? value)
    {
        var converted = value is not null ? Convert.ChangeType(value, type, null) : DBNull.Value;

        return Expression.Equal(property, Expression.Constant(converted, type));
    }

    public static Expression Compare(ExpressionType comparison, Expression property, object? value)
    {
        var targetType = property.Type;
        var converted = value is not null ? Convert.ChangeType(value, targetType, null) : DBNull.Value;

        return Expression.MakeBinary(comparison, property, Expression.Constant(converted, targetType));
    }

    public static Expression In(Expression property, Type type, object? value)
    {
        if (value is not IEnumerable values || value is string)
        {
            throw new InvalidOperationException("IN operator requires an array");
        }

        if (property.Type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.Type))
        {
            var elementType = property.Type.GetGenericArguments()[0];

            var listType = typeof(List<>).MakeGenericType(elementType);
            var valList = Activator.CreateInstance(listType)!;
            var add = listType.GetMethod(nameof(IList.Add))!;

            foreach (var v in values)
            {
                add.Invoke(valList, [Convert.ChangeType(v, elementType, null)]);
            }

            var anyParam = Expression.Parameter(elementType, "p");
            var containsCall = Expression.Call(
                Expression.Constant(valList),
                listType.GetMethod(nameof(IList.Contains))!,
                anyParam
            );
            var lambda = Expression.Lambda(containsCall, anyParam);

            return Expression.Call(typeof(Enumerable), nameof(Enumerable.Any), [elementType], property, lambda);
        }
        else
        {
            var elementType = property.Type;
            var listType = typeof(List<>).MakeGenericType(elementType);
            var valList = Activator.CreateInstance(listType)!;
            var add = listType.GetMethod(nameof(IList.Add))!;

            foreach (var v in values)
            {
                add.Invoke(valList, [Convert.ChangeType(v, elementType, null)]);
            }

            return Expression.Call(Expression.Constant(valList), listType.GetMethod(nameof(IList.Contains))!, property);
        }
    }

    public static Expression Contains(Expression property, Type type, object? value)
    {
        if (type != typeof(string))
        {
            throw new InvalidOperationException("Contains only valid on string fields");
        }

        var values = value switch
        {
            object?[] ja => ja,
            IEnumerable e and not string => e.Cast<object?>(),
            null => [],
            _ => [value],
        };

        Expression? combined = null;

        foreach (var v in values)
        {
            var toValue = v?.ToString();

            if (string.IsNullOrWhiteSpace(toValue))
            {
                continue;
            }

            var call = Expression.Call(
                property,
                nameof(string.Contains),
                Type.EmptyTypes,
                Expression.Constant(toValue)
            );

            combined = combined != null ? Expression.OrElse(combined, call) : call;
        }

        return combined ?? Expression.Constant(true);
    }
}
