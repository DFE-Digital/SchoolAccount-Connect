using System.Linq.Expressions;

namespace SchoolAccount.Application.Features.Shared.Filtering.Models;

public sealed class ParameterReplaceVisitor(ParameterExpression from, ParameterExpression to) : ExpressionVisitor
{
    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node == from ? to : base.VisitParameter(node);
    }
}
