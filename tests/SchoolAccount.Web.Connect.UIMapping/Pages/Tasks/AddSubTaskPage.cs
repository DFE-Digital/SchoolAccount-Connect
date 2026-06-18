using Microsoft.Playwright;
using PlaywrightTests.DfE.UIMapping.Utils;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.Kernel.Utils;

namespace PlaywrightTests.DfE.UIMapping.Pages.Tasks;

/// <summary>
/// Page model allowing a sub-task to be added.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class AddSubTaskPage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    public ILocator SubTaskName => PageContext.InputLocatorById("subTaskName");
    public ILocator MandatorySubTaskRadioButton => PageContext.InputLocatorById("taskRequirement-1");

    private ILocator Team => PageContext.InputLocatorById("team"); 
    private ILocator AddATagButton => PageContext.ButtonLocatorByText("Add a tag");
    private ILocator SaveAndContinueButton => PageContext.ButtonLocatorByText("Save and continue");
    private ILocator SaveAndFinishLaterButton => PageContext.ButtonLocatorByText("Save and finish later");

    public async Task<CreateSubTaskDetailsConfirmPage> ClickSaveAndContinueAsync()
    {
        await ClickAsync(SaveAndContinueButton);

        return new CreateSubTaskDetailsConfirmPage(PageContext, RunContext);
    }

    public async Task ClickSaveAndFinishLaterAsync()
    {
        await ClickAsync(SaveAndFinishLaterButton);
    }

    public async Task<AddInsightTagsPage> ClickAddATagAsync()
    {
        await AddATagButton.ClickAsync();

        return new AddInsightTagsPage(PageContext, RunContext);
    }

    public async Task EnterLinkedTeam(string teamName)
    {
        await FieldHelpers.EnterLinkedData(Team, PageContext, teamName);
    }
}
