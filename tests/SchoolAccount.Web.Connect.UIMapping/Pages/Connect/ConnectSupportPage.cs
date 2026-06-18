using Microsoft.Playwright;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.DfE.UIMapping.Pages.Connect.Components;
using PlaywrightTests.DfE.UIMapping.Pages.Connect.Components.Support;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect;

public class ConnectSupportPage(IPage page, IRunContext runContext) : ConnectBasePage(page, runContext)
{
    //This is the full Support Page.
    public DataAnalyticsLinks DataAnalytics { get; } = new DataAnalyticsLinks(page, runContext);
    public StatutoryGuidanceLinks StatutoryGuidance { get; } = new StatutoryGuidanceLinks(page, runContext);
    public AdviceSupportLinks AdviceSupport { get; } = new AdviceSupportLinks(page, runContext);
}