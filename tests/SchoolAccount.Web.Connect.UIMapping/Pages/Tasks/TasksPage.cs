using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.Kernel.Utils;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightTests.DfE.UIMapping.Pages.Tasks;

/// <summary>
/// Page model showing details of all the Tasks.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class TasksPage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    private ILocator AddTaskButton => PageContext.ButtonLocatorByText("Create a task");
    private ILocator TasksTable => PageContext.Locator("table[id='sortTable']");
    private ILocator TasksTableRows => TasksTable.Locator("tbody tr");

    // Actions
    public async Task<AddTaskPage> ClickCreateATaskAsync()
    {
        await AddTaskButton.ClickAsync();

        return new AddTaskPage(PageContext, RunContext);
    }

    public async Task AssertTaskRowByNameAsync(string taskName)
    {
        var taskRow = GetTaskRowByTaskName(taskName);
        await Expect(taskRow).ToBeVisibleAsync();
    }

    public async Task<TaskDetailsPage> ClickTaskLinkByNameAsync(string taskName)
    {
        var taskRow = GetTaskRowByTaskName(taskName);
        var taskLink = taskRow.Locator("td").First.Locator("a");
        await taskLink.ClickAsync();
        await WaitForPageLoadAsync();

        return new TaskDetailsPage(PageContext, RunContext);
    }

    public async Task VerifyPageTitleAsync()
    {
        await AssertTitle("Tasks");
    }

    private ILocator GetTaskRowByTaskName(string taskName)
    {
        return TasksTableRows.Filter(new() { HasText = taskName });
    }
}