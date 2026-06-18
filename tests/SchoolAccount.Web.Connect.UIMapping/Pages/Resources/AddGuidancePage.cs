using Microsoft.Playwright;
using PlaywrightTests.DfE.UIMapping.Forms;
using PlaywrightTests.DfE.UIMapping.Utils;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Resources;

/// <summary>
/// Page model for adding a guidance resource.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class AddGuidancePage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    public ILocator GuidanceName => PageContext.Locator("input[id='guidanceName']");
    public ILocator GuidanceLink => PageContext.Locator("input[id='guidanceLink']");
    public ILocator Service => PageContext.Locator("input[id='service']");
    public ILocator StatutorySelect => PageContext.Locator("input[id='statutory-type']");
    public ILocator NonstatSelect => PageContext.Locator("input[id='non-statutory-type']");
    public ILocator DayInput => PageContext.Locator("input[id='lastGovUpdate-day']");
    public ILocator MonthInput => PageContext.Locator("input[id='lastGovUpdate-month']");
    public ILocator YearInput => PageContext.Locator("input[id='lastGovUpdate-year']");
    public ILocator ActiveResource => PageContext.Locator("input[id='resourceStatus-active']");
    public ILocator WithdrawnResource => PageContext.Locator("input[id='resourceStatus-withdrawn']");
    public ILocator ContinueButton => PageContext.Locator("button").Filter(new() { HasText = "Continue" });
    public ILocator CancelButton => PageContext.Locator("a").Filter(new() { HasText = "Cancel" });

    public async Task EnterGuidanceName(string guidanceName)
    {
        await GuidanceName.FillAsync(guidanceName);
    }

    public async Task EnterGuidanceLink(string guidanceLink)
    {
        await GuidanceLink.FillAsync(guidanceLink);
    }

    // TODO: DRY out to an AddResourceJourneyHelpers class?
    public async Task EnterLinkedTeam(string teamName)
    {
        await FieldHelpers.EnterLinkedData(Service, PageContext, teamName);
    }

    public async Task GuidanceTypeSelect(int guidanceTypeId)
    {
        switch (guidanceTypeId)
        {
            case 1:
                await StatutorySelect.ClickAsync();
                break;
            case 2:
                await NonstatSelect.ClickAsync();
                break;
            default:
                throw new ArgumentException($"Invalid guidance type ID: {guidanceTypeId}. Valid values are 1 (Statutory) or 2 (Non-Statutory).");
        }
    }

    public async Task GuidanceTypeSelectByText(string guidanceTypeText)
    {
        if (guidanceTypeText.ToLower().Contains("statutory"))
        {
            await GuidanceTypeSelect(1);
        }
        else
        {
            await GuidanceTypeSelect(2);
        }
    }

    public async Task EnterUpdateDate(string dateString)
    {
        var dateParts = dateString.Split('-');
        var year = dateParts[0];
        var month = dateParts[1];
        var day = dateParts[2];

        await DayInput.FillAsync(day);
        await MonthInput.FillAsync(month);
        await YearInput.FillAsync(year);
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
        ArgumentNullException.ThrowIfNull(nameof(resourceFormData));

        await EnterGuidanceName(resourceFormData.ResourceName);
        await EnterGuidanceLink(resourceFormData.DigitalLink);
        await GuidanceTypeSelect(resourceFormData.GuidanceTypeId!.Value);

        await EnterUpdateDate(resourceFormData.GovUkLastUpdated);
        await EnterLinkedTeam(resourceFormData.TeamName);
        await SelectResourceStatus(resourceFormData.ResourceStatusId);
        await ContinueButton.ClickAsync();
    }
}