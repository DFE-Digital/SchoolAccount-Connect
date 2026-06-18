using Microsoft.Playwright;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Tasks;

/// <summary>
/// Page model allowing related tasks to be explicitly ordered.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class SpecificOrderRelatedTasksPage(IPage page, IRunContext runContext) : SpecificOrderBasePage(page, runContext)
{
    public async Task VerifyPageTitleAsync()
    {
        await AssertTitle("Order related tasks");
    }
}
