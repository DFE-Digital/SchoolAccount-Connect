using System.Collections.ObjectModel;
using System.Globalization;
using SchoolAccount.Application.Features.Tasks.GetById;
using SchoolAccount.Domain.Conditions;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Organisations;

namespace SchoolAccount.Application.Extensions;

public static class ConditionExtensions
{
    public static bool? Compute(IConditionObject condition, IOrganisation? organisation)
    {
        if (organisation is not EstablishmentOrganisation establishment)
        {
            return null;
        }

        var metric = establishment.Conditions.FirstOrDefault(x => x.Identifier == condition.Identifier);

        if (metric == null)
        {
            return null;
        }

        var metricValue = int.Parse(metric.Value.ToString() ?? "0", CultureInfo.InvariantCulture);
        var conditionValue = int.Parse(condition.Value?.ToString() ?? "0", CultureInfo.InvariantCulture);

        return condition.ComparitorType switch
        {
            ConditionComparitorType.Equal => metricValue == conditionValue,
            ConditionComparitorType.NotEqual => metricValue != conditionValue,
            ConditionComparitorType.GreaterThan => metricValue > conditionValue,
            ConditionComparitorType.GreaterThanOrEqual => metricValue >= conditionValue,
            ConditionComparitorType.LessThan => metricValue < conditionValue,
            ConditionComparitorType.LessThanOrEqual => metricValue <= conditionValue,
            _ => false
        };
    }
    
    public static string DetermineColour(this IConditionObject condition, IOrganisation? establishment)
    {
        return Compute(condition, establishment) switch
        {
            true => "govuk-tag--green",
            false => "govuk-tag--red",
            _ => "govuk-tag--grey"
        };
    }
    
    public static string Comparitor(this IConditionObject condition)
    {
        return condition.ComparitorType.Comparitor();
    }
    
    public static string Comparitor(this ConditionComparitorType comparitor)
    {
        return comparitor switch
        {
            ConditionComparitorType.Equal => "=",
            ConditionComparitorType.NotEqual => "!=",
            ConditionComparitorType.GreaterThan => ">",
            ConditionComparitorType.GreaterThanOrEqual => ">=",
            ConditionComparitorType.LessThan => "<",
            ConditionComparitorType.LessThanOrEqual => "<=",
            _ => string.Empty
        };
    }

    public static string DetermineColour(this Collection<IConditionObject> conditions,
        EstablishmentOrganisation? establishment)
    {
        if (conditions.All(x => Compute(x, establishment) == true))
        {
            return "govuk-tag--green";
        }

        if (conditions.Any(x => Compute(x, establishment) == true))
        {
            return "govuk-tag--orange";
        }

        return "govuk-tag--red";
    }
}