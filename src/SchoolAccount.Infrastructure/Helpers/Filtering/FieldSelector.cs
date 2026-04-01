using System.Linq.Expressions;

namespace SchoolAccount.Infrastructure.Helpers.Filtering;

public class FieldSelector : Dictionary<string, LambdaExpression>;
