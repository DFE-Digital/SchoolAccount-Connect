using System.Linq.Expressions;

namespace SchoolAccount.Application.Features.Shared.Filtering;

public class FieldSelector : Dictionary<string, LambdaExpression>;
