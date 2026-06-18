using Microsoft.Playwright;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect.Components;

public class TaskItem
{
    public string TaskName { get; set; } = string.Empty;
    public string SubTasks { get; set; } = string.Empty;
    public string DueDate { get; set; } = string.Empty;
    public ILocator LinkLocator { get; set; } = null!;
}