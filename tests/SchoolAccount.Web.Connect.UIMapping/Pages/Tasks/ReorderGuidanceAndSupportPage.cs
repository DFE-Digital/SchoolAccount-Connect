using Microsoft.Playwright;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Tasks;

/// <summary>
/// Page model allowing Guidance and Support to be reordered.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
/// <remarks>We've retained this as an empty class in case its functionality diverges from the base reorder page.</remarks>
public class ReorderGuidanceAndSupportPage(IPage page, IRunContext runContext) : ReorderBasePage(page, runContext)
{
    public async Task VerifyPageTitleAsync()
    {
        await AssertTitle("Guidance and support order options");
    }
}
