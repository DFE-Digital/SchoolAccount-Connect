using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.Utils;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect.Components;

public class FullCalendarOfTasksLink(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    public ILocator Link => PageContext.AnchorLocatorByText("See the full calendar of tasks");
}