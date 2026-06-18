using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.Kernel.Utils;

namespace PlaywrightTests.DfE.UIMapping.Pages.Tasks;

/// <summary>
/// Page model allowing the first page of Audience tags to be amended.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class AddAudienceTagsPage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    public ILocator AllPhasesOfEducationCheckbox => PageContext.InputLocatorById("all");
    public ILocator AcademyInstitutionCheckbox => PageContext.InputLocatorById("institutionType-0");
    private ILocator SaveAndContinueButton => PageContext.ButtonLocatorByText("Save and continue");

    public async Task<AddAudience2TagsPage> ClickSaveAndContinueAsync()
    {
        await SaveAndContinueButton.ClickAsync();

        return new AddAudience2TagsPage(PageContext, RunContext);
    }
}
