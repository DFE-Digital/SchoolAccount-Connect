using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.Kernel.Utils;

namespace PlaywrightTests.DfE.UIMapping.Pages.Tasks;

/// <summary>
/// Page model allowing the second page of Audience tags to be amended.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class AddAudience2TagsPage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    private ILocator ConfirmTagsButton => PageContext.ButtonLocatorByText("Confirm tags and go back to subtask");

    public async Task ClickConfirmAsync()
    {
        await ConfirmTagsButton.ClickAsync();
    }
}
