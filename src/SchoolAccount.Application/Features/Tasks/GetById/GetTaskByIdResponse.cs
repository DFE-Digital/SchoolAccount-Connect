using System.Collections.ObjectModel;
using System.Globalization;
using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Kernel.Organisations;

namespace SchoolAccount.Application.Features.Tasks.GetById;

public sealed record GetTaskByIdResponse
{
    public long Id { get; init; }

    public string? ReferenceNo { get; init; }

    public string Name { get; init; } = string.Empty;

    public Requirement? Requirement { get; init; }

    public DateTime DateUpdated { get; init; }

    public string UpdatedBy { get; init; } = string.Empty;

    public DateTime? SubTaskLastUpdated => GetSubTaskLastUpdated();

    public IReadOnlyCollection<GetTaskByIdResponseSubtask> SubTasks { get; init; } = [];

    public IReadOnlyCollection<GetTaskByIdResponseResource> Resources { get; init; } = [];

    public IReadOnlyCollection<GetTaskByIdResponseRelatedTask> RelatedTasks { get; init; } = [];

    private DateTime? GetSubTaskLastUpdated()
    {
        return SubTasks.OrderByDescending(st => st.DateUpdated).FirstOrDefault()?.DateUpdated;
    }
}

public sealed record GetTaskByIdResponseSubtask
{
    public long Id { get; init; }

    public string? ReferenceNo { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public DateOnly? StartDate { get; init; }

    public bool? StartDateIsExact { get; init; }

    public DateOnly? DueDate { get; init; }

    public bool? DueDateIsExact { get; init; }

    public string AvailabilityLabel { get; init; } = string.Empty;

    public string DueDateLabel { get; init; } = string.Empty;

    public Requirement Requirement { get; init; }

    public WorkflowState WorkflowState { get; init; }

    public DateTime DateUpdated { get; init; }

    public string UpdatedBy { get; init; } = string.Empty;

    public bool HasDueDateLabel => !string.IsNullOrWhiteSpace(DueDateLabel);

    public bool HasAvailabilityLabel => !string.IsNullOrWhiteSpace(AvailabilityLabel);

    public bool HasDescription => !string.IsNullOrWhiteSpace(Description);

    public bool IsOptional => Requirement == Requirement.Optional;

    public DateOnly? SortingDate => DueDate ?? StartDate;

    public string? ResourceName { get; init; } = string.Empty;

    public string? ResourceLink { get; init; } = string.Empty;

    public bool HasResourceLink => !string.IsNullOrWhiteSpace(ResourceLink);
    
    public Collection<GetTaskByIdResponseCondition> Conditions { get; init; } = [];
    
    public bool HasConditions => Conditions.Count > 0;
}

public sealed record GetTaskByIdResponseCondition
{
    public required string Identifier { get; init; }
    public required SubTaskConditionComparitorType ComparitorType { get; init; }
    public required object? Value { get; init; }

    public static bool? Compute(GetTaskByIdResponseCondition condition, EstablishmentOrganisation? establishment)
    {
        if (establishment == null)
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
            SubTaskConditionComparitorType.Equal => metricValue == conditionValue,
            SubTaskConditionComparitorType.NotEqual => metricValue != conditionValue,
            SubTaskConditionComparitorType.GreaterThan => metricValue > conditionValue,
            SubTaskConditionComparitorType.GreaterThanOrEqual => metricValue >= conditionValue,
            SubTaskConditionComparitorType.LessThan => metricValue < conditionValue,
            SubTaskConditionComparitorType.LessThanOrEqual => metricValue <= conditionValue,
            _ => false
        };
    }
    
    public string DetermineColour(EstablishmentOrganisation? establishment)
    {
        return Compute(this, establishment) switch
        {
            true => "govuk-tag--green",
            false => "govuk-tag--red",
            _ => "govuk-tag--grey"
        };
    }
    
    public string Comparitor()
    {
        return ComparitorType switch
        {
            SubTaskConditionComparitorType.Equal => "=",
            SubTaskConditionComparitorType.NotEqual => "!=",
            SubTaskConditionComparitorType.GreaterThan => ">",
            SubTaskConditionComparitorType.GreaterThanOrEqual => ">=",
            SubTaskConditionComparitorType.LessThan => "<",
            SubTaskConditionComparitorType.LessThanOrEqual => "<=",
            _ => "NOTDEFINED"
        };
    }

    public static string DetermineColour(Collection<GetTaskByIdResponseCondition> conditions,
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

public sealed record GetTaskByIdResponseResource
{
    public required string Name { get; init; }

    public string? Link { get; init; }

    public bool HasLink => !string.IsNullOrWhiteSpace(Link);
}

public sealed record GetTaskByIdResponseRelatedTask
{
    public long Id { get; init; }

    public string Name { get; init; } = string.Empty;
}
