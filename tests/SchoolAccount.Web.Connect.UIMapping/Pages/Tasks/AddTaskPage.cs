using Microsoft.Playwright;
using PlaywrightTests.DfE.UIMapping.Utils;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.Kernel.Utils;

namespace PlaywrightTests.DfE.UIMapping.Pages.Tasks;

/// <summary>
/// Page model allowing a new task to be created.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class AddTaskPage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    public ILocator TaskName => PageContext.InputLocatorById("taskName");
    public ILocator TaskDescription => PageContext.Locator("textarea[id='taskDescription']");
    public ILocator AddCategoryButton => PageContext.ButtonLocatorById("categories");
    public ILocator CreateSubTaskButton => PageContext.ButtonLocatorById("subtasks");
    private ILocator SaveAndContinueButton => PageContext.ButtonLocatorByText("Save and continue to publish");
    private ILocator SaveAndFinishLaterButton => PageContext.ButtonLocatorByText("Finish and save as draft");
    private ILocator BackButton => PageContext.Locator("a.govuk-back-link");
    private ILocator CancelButton => PageContext.Locator("a").Filter(new() { HasText = "Cancel" });
    private ILocator ReorderGuidanceAndSupportButton => PageContext.ButtonLocatorByText("Reorder guidance and support");
    private ILocator ReorderRelatedTasksButton => PageContext.ButtonLocatorByText("Reorder related tasks");

    private ILocator RelatedTask => PageContext.InputLocatorById("taskSearch");
    private ILocator GuidanceAndSupport => PageContext.InputLocatorById("resourceSearch");

    public async Task VerifyPageTitleAsync()
    {
        await AssertTitle("Create a task");
    }

    public async Task<SelectCategoriesPage> ClickAddCategoryAsync()
    {
        await AddCategoryButton.ClickAsync();

        return new SelectCategoriesPage(PageContext, RunContext);
    }

    public async Task<AddSubTaskPage> ClickCreateASubTaskAsync()
    {
        await ClickAsync(CreateSubTaskButton);

        return new AddSubTaskPage(PageContext, RunContext);
    }

    public async Task<PublishTaskToDfEConnectPage> ClickSaveAndContinueAsync()
    {
        await SaveAndContinueButton.ClickAsync();

        return new PublishTaskToDfEConnectPage(PageContext, RunContext);
    }

    public async Task ClickSaveAndFinishLaterAsync()
    {
        await SaveAndFinishLaterButton.ClickAsync();
    }

    public async Task ClickBackAsync()
    {
        await BackButton.ClickAsync();
    }

    public async Task ClickCancelAsync()
    {
        await CancelButton.ClickAsync();
    }

    public async Task<ReorderGuidanceAndSupportPage> ClickReorderGuidanceAndSupportAsync()
    {
        await ClickAsync(ReorderGuidanceAndSupportButton);
        await ExplicitWaitWithReason(200, "This is necessary because of the issue with attaching the event listeners in Angular.");

        return new ReorderGuidanceAndSupportPage(PageContext, RunContext);
    }

    public async Task<ReorderRelatedTasksPage> ClickReorderRelatedTasksAsync()
    {
        await ClickAsync(ReorderRelatedTasksButton);
        await ExplicitWaitWithReason(200, "This is necessary because of the issue with attaching the event listeners in Angular.");

        return new ReorderRelatedTasksPage(PageContext, RunContext);
    }

    public async Task EnterGuidanceAndSupport(string resourceName)
    {
        await FieldHelpers.EnterLinkedData(GuidanceAndSupport, PageContext, resourceName);
    }

    public async Task EnterRelatedTask(string taskName)
    {
        await FieldHelpers.EnterLinkedData(RelatedTask, PageContext, taskName);
    }
}
