using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.Utils;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect.Components;

public class ExploreTasksComponent(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    public ILocator AllTasksLink => PageContext.AnchorLocatorByText("All tasks");
    public ILocator FinanceLink => PageContext.AnchorLocatorByText("Finance");
    public ILocator FundingLink => PageContext.AnchorLocatorByText("Funding");
    public ILocator ProcurementLink => PageContext.AnchorLocatorByText("Procurement");
    public ILocator StaffHRLink => PageContext.AnchorLocatorByText("Staff and HR");
    public ILocator AcademyTrustLink => PageContext.AnchorLocatorByText("Academy Trust Handbook");
    public ILocator PupilsLink => PageContext.AnchorLocatorByText("Pupils");
    public ILocator Past12MonthsLink => PageContext.AnchorLocatorByText("Past 12 months");
}