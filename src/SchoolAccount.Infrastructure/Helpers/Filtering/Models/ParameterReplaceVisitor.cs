using System.Linq.Expressions;

namespace SchoolAccount.Infrastructure.Helpers.Filtering.Models;

public sealed class ParameterReplaceVisitor(ParameterExpression from, ParameterExpression to) : ExpressionVisitor
{
    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node == from ? to : base.VisitParameter(node);
    }
}