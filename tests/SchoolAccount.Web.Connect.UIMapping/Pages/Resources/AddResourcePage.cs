using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.DfE.UIMapping.Forms;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Resources;

/// <summary>
/// Page model for adding a new resource.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class AddResourcePage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    public ILocator GuidanceSelect => PageContext.Locator("input[value='guidance']");
    public ILocator DigitalServiceSelect => PageContext.Locator("input[value='service']");
    public ILocator ContinueButton => PageContext.Locator("button").Filter(new() { HasText = "Continue" });
    public ILocator CancelButton => PageContext.Locator("a").Filter(new() { HasText = "Cancel" });

    public async Task SelectResourceType(int resourceTypeId)
    {
        switch (resourceTypeId) // Use 1 for Guidance and 2 for Digital Service. This matches the DB entries.
        {
            case 1:
                await GuidanceSelect.ClickAsync();
                break;
            case 2:
                await DigitalServiceSelect.ClickAsync();
                break;
            default:
                throw new ArgumentException($"Invalid resource type ID: {resourceTypeId}. Valid values are 1 (Guidance) or 2 (Digital Service).");
        }

        await ContinueButton.ClickAsync();
    }

    // This accepts guidance or service as string input
    public async Task SelectResourceTypeByName(string resourceTypeName)
    {
        if (resourceTypeName.ToLower() == "guidance")
        {
            await SelectResourceType(1);
        }
        else
        {
            await SelectResourceType(2);
        }
    }

    public async Task AddByFormData(ResourceFormData resourceFormData)
    {
        ArgumentNullException.ThrowIfNull(nameof(resourceFormData));

        await SelectResourceType(resourceFormData.ResourceTypeId);

        if (resourceFormData.ResourceTypeId == 1)
        {
            var addGuidancePage = new AddGuidancePage(PageContext, RunContext);
            await addGuidancePage.AddByFormData(resourceFormData);
        }
        else if (resourceFormData.ResourceTypeId == 2)
        {
            var addDigitalServicePage = new AddDigitalServicePage(PageContext, RunContext);
            await addDigitalServicePage.AddByFormData(resourceFormData);
        }

        // TODO: This is adopting the approach where this page is the orchestrator.
        // An alternative approach is to chain the pages, so the AddGuidancePage.AddByFormData
        // then creates the AddResourceConfirmationPage and calls its AddByFormData method,
        // which would call the ClickSubmitButtonAsync method.
        var addResourceConfirmationPage = new AddResourceConfirmationPage(PageContext, RunContext);
        await addResourceConfirmationPage.ClickSubmitButtonAsync();
    }
}