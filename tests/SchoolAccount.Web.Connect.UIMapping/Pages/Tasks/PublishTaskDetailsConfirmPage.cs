using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.Kernel.Utils;

namespace PlaywrightTests.DfE.UIMapping.Pages.Tasks;

/// <summary>
/// Page model allowing the task details to be confirmed prior to publishing.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class PublishTaskDetailsConfirmPage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    private ILocator ConfirmAndPublishTaskButton => PageContext.ButtonLocatorByType("submit");

    public async Task ClickConfirmAndPublishAsync()
    {
        await ConfirmAndPublishTaskButton.ClickAsync();
    }
}