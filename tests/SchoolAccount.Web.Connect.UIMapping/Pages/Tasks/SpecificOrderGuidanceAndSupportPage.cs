using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Tasks;

/// <summary>
/// Page model allowing guidance and support to be explicitly ordered.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class SpecificOrderGuidanceAndSupportPage(IPage page, IRunContext runContext) : SpecificOrderBasePage(page, runContext)
{
    public async Task VerifyPageTitleAsync()
    {
        await AssertTitle("Order guidance and support");
    }
}
