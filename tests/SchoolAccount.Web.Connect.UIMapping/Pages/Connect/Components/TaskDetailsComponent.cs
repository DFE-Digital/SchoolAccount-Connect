using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect.Components;

public class TaskDetailsComponent(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    // Locators
    public ILocator TaskTitle => PageContext.Locator("h1");
    public ILocator TaskDetails => PageContext.Locator("p.govuk-body");
    public ILocator UpcomingTasks => PageContext.Locator("a[id='tab_upcoming']");
    public ILocator PreviousTasks => PageContext.Locator("a[id='tab_previous']");
}