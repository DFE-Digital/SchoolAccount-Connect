using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.Utils;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect.Components.Support;

public class DataAnalyticsLinks(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    public ILocator FinancialInsights => PageContext.AnchorLocatorByText("Access the Financial Benchmarking and Insights Tool");
    public ILocator FinancialManagement => PageContext.AnchorLocatorByText("Compare financial management systems");
    public ILocator EducationData => PageContext.AnchorLocatorByText("View your education data");
}