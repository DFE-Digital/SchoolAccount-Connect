using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect.Components;

public class TasksByDateComponent(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    public ILocator TaskListItems => PageContext.Locator("li.govuk-task-list__item.govuk-task-list__item--with-link");
    public ILocator NextPage => PageContext.Locator("a[rel='next']");
    public ILocator PreviousPage => PageContext.Locator("a[rel='prev']");


    private ILocator GetLinkFromTask(ILocator listItem)
    {
        return listItem.Locator("a.govuk-link.govuk-task-list__link");
    }
    private ILocator GetSubTasksFromTask(ILocator listItem)
    {
        return listItem.Locator("div.govuk-task-list__hint").Nth(0);
    }
    private ILocator GetDueDateFromTask(ILocator listItem)
    {
        return listItem.Locator("div.govuk-task-list__hint").Nth(1).Locator("strong");
    }
    public async Task<List<TaskItem>> GetAllTaskItemsAsync()
    {
        var taskItems = new List<TaskItem>();
        var totalTaskListItems = await TaskListItems.CountAsync();

        for (int taskIndex = 0; taskIndex < totalTaskListItems; taskIndex++)
        {
            var listItem = TaskListItems.Nth(taskIndex);

            taskItems.Add(new TaskItem
            {
                TaskName = await GetLinkFromTask(listItem).TextContentAsync() ?? string.Empty,
                SubTasks = await GetSubTasksFromTask(listItem).TextContentAsync() ?? string.Empty,
                DueDate = await GetDueDateFromTask(listItem).TextContentAsync() ?? string.Empty,
                LinkLocator = GetLinkFromTask(listItem)
            });
        }

        return taskItems;
    }

    public async Task<TaskItem?> GetTaskItemByNameAsync(string taskName)
    {
        var allTasks = await GetAllTaskItemsAsync();
        RunContext.Log($"Looking for task with name '{taskName}' among {allTasks.Count} tasks");
        //Create a loop to print the TaskName of all tasks in allTasks
        foreach (var task in allTasks)
        {
            RunContext.Log($"Task found: '{task.TaskName.Trim()}'");
        }
        return allTasks.First(t => t.TaskName.Trim().Equals(taskName, StringComparison.OrdinalIgnoreCase));
    }    

    public async Task ClickTaskByNameAsync(string taskName)
    {
        var task = await GetTaskItemByNameAsync(taskName);
        if (task != null)
        {
            await task.LinkLocator.ClickAsync();
        }
        else
        {
            throw new InvalidOperationException($"Task '{taskName}' not found in the task list");
        }
    }
}