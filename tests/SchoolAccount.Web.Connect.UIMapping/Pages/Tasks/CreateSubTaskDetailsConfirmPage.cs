using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.Kernel.Utils;

namespace PlaywrightTests.DfE.UIMapping.Pages.Tasks;

/// <summary>
/// Page model allowing the sub-task details to be confirmed.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class CreateSubTaskDetailsConfirmPage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    private ILocator ConfirmAndReturnToTaskButton => PageContext.ButtonLocatorByText("Confirm and return to task");

    public async Task ClickConfirmAndReturnToTaskAsync()
    {
        await ConfirmAndReturnToTaskButton.ClickAsync();
    }
}
