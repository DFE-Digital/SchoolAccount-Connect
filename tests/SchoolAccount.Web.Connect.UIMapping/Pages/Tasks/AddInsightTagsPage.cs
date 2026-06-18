using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.Kernel.Utils;

namespace PlaywrightTests.DfE.UIMapping.Pages.Tasks;

/// <summary>
/// Page model allowing the insight tags to be amended.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class AddInsightTagsPage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    public ILocator ApplyForSomethingActivityCheckbox => PageContext.InputLocatorById("activity-0");
    public ILocator ComplyingWithRegulationsPurposeCheckbox => PageContext.InputLocatorById("purpose-0");
    public ILocator TransactionalYesRadioButton => PageContext.InputLocatorById("transactional-Transactional");
    public ILocator SizeLargeRadioButton => PageContext.InputLocatorById("size-19");
    private ILocator SaveAndContinueButton => PageContext.ButtonLocatorByText("Save and continue");

    public async Task<AddAudienceTagsPage> ClickSaveAndContinueAsync()
    {
        await SaveAndContinueButton.ClickAsync();

        return new AddAudienceTagsPage(PageContext, RunContext);
    }
}
