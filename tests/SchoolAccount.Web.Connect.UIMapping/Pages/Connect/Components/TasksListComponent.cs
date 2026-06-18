using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect.Components;

public class TaskListComponent(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    // Locators
    public ILocator TasksList => PageContext.Locator("ul.gem-c-cards__list.gem-c-cards__list--one-column");
    public ILocator SubTaskNames => TasksList.Locator("span.sub-task-name");
    // Methods
    public async Task<List<string>> GetAllSubTaskNamesAsync()
    {
        var subTaskNamesList = new List<string>();
        var count = await SubTaskNames.CountAsync();

        for (int i = 0; i < count; i++)
        {
            var subTaskName = await SubTaskNames.Nth(i).TextContentAsync();
            subTaskNamesList.Add(subTaskName?.Trim() ?? string.Empty);
        }

        return subTaskNamesList;
    }
    public async Task<int> GetSubTaskCountAsync()
    {
        return await SubTaskNames.CountAsync();
    }
    public ILocator GetSubTaskByName(string subTaskName)
    {
        return SubTaskNames.Filter(new() { HasText = subTaskName });
    }
    public async Task ClickSubTaskByNameAsync(string subTaskName)
    {
        await GetSubTaskByName(subTaskName).ClickAsync();
    }
}