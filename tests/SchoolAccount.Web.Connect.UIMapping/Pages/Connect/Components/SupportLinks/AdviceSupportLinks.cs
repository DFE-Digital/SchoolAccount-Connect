using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.Utils;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect.Components.Support;

public class AdviceSupportLinks(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    public ILocator BuyingForSchools => PageContext.AnchorLocatorByText("Buying for schools");
    public ILocator DFEHelp => PageContext.AnchorLocatorByText("DfE help centre");
    public ILocator FinancialGoodPractice => PageContext.AnchorLocatorByText("Financial good practice guides for trusts");
    public ILocator FundingAllocation => PageContext.AnchorLocatorByText("Funding allocation");
    public ILocator ICFPPlanning => PageContext.AnchorLocatorByText("Integrated curriculum and financial planning (ICFP)");
    public ILocator RiskProtection => PageContext.AnchorLocatorByText("Risk protection arrangement");
    public ILocator ResourceManagement => PageContext.AnchorLocatorByText("School resource management advisers");
}