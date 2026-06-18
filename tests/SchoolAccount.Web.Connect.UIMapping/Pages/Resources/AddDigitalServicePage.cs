using Microsoft.Playwright;
using PlaywrightTests.DfE.UIMapping.Forms;
using PlaywrightTests.DfE.UIMapping.Utils;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Resources;

/// <summary>
/// Page model for adding a digital service resource.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class AddDigitalServicePage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    public ILocator ServiceName => PageContext.Locator("input[id='serviceName']");
    public ILocator ServiceLink => PageContext.Locator("input[id='serviceLink']");
    public ILocator Service => PageContext.Locator("input[id='service']");
    public ILocator ActiveResource => PageContext.Locator("input[id='resourceStatus-active']");
    public ILocator WithdrawnResource => PageContext.Locator("input[id='resourceStatus-withdrawn']");
    public ILocator ContinueButton => PageContext.Locator("button").Filter(new() { HasText = "Continue" });
    public ILocator CancelButton => PageContext.Locator("a").Filter(new() { HasText = "Cancel" });

    public async Task EnterLinkedTeam(string teamName)
    {
        await FieldHelpers.EnterLinkedData(Service, PageContext, teamName);
    }

    // TODO: DRY out to an AddResourceJourneyHelpers class?
    public async Task SelectResourceStatus(int resourceStatusId)
    {
        switch (resourceStatusId)
        {
            case 2:
                await ActiveResource.ClickAsync();
                break;
            case 3:
            case 4:
                await WithdrawnResource.ClickAsync();
                break;
            default:
                throw new ArgumentException($"Invalid resource status ID: {resourceStatusId}. Valid values are 2 (Active), 3 or 4 (Withdrawn).");
        }
    }

    // TODO: DRY out to an AddResourceJourneyHelpers class?
    public async Task SelectResourceStatusByText(string resourceStatusText)
    {
        var lowerText = resourceStatusText.ToLower();

        if (lowerText.Contains("active"))
        {
            await SelectResourceStatus(2);
        }
        else if (lowerText.Contains("withdrawn") || lowerText.Contains("decommissioned"))
        {
            await SelectResourceStatus(4);
        }
    }

    public async Task AddByFormData(ResourceFormData resourceFormData)
    {
        await ServiceName.FillAsync(resourceFormData.ResourceName);
        await ServiceLink.FillAsync(resourceFormData.DigitalLink);
        await EnterLinkedTeam(resourceFormData.TeamName);
        await SelectResourceStatus(resourceFormData.ResourceStatusId);
        await ContinueButton.ClickAsync();
    }
}