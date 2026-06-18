using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.Utils;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect.Components.Base;

public class NavBarComponent(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    public ILocator HomeButton => PageContext.AnchorLocatorByText("Home");
    public ILocator COTButton => PageContext.AnchorLocatorByText("Calendar of tasks");
    public ILocator SupportButton => PageContext.AnchorLocatorByText("Support");
}