namespace PlaywrightTests.DfE.UIMapping.Forms;

public static class FormHelpers
{
    public static readonly Dictionary<string, int> RequirementIdMapping = new()
    {
        { "Mandatory", 1 },
        { "Conditional", 2 },
        { "Optional", 3 }
    };

    public static readonly Dictionary<string, int> WorkflowStateMapping = new()
    {
        { "Draft", 1 },
        { "Queued", 2 },
        { "Published", 3 },
        { "Expired", 4 },
        { "Archived", 5 }
    };

    // Helper methods for converting between friendly names and database IDs
    public static int GetRequirementId(string requirement)
    {
        return RequirementIdMapping.TryGetValue(requirement, out int id) ? id : 3; // Default to Optional
    }
    
    public static int GetWorkflowStateId(string workflowState)
    {
        return WorkflowStateMapping.TryGetValue(workflowState, out int id) ? id : 1; // Default to Draft
    }

    // Helper methods to convert from IDs back to friendly names (for UI integration)
    public static string GetRequirementName(int requirementId)
    {
        return RequirementIdMapping.FirstOrDefault(x => x.Value == requirementId).Key ?? "Optional";
    }

    public static string GetWorkflowStateName(int workflowStateId)
    {
        return WorkflowStateMapping.FirstOrDefault(x => x.Value == workflowStateId).Key ?? "Draft";
    }
}