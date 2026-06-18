using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.DfE.UIMapping.Forms;

using static Microsoft.Playwright.Assertions;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Resources;

/// <summary>
/// Page model for the resource details page.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class ResourceDetailsPage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    private ResourceFormData? _resourceFormData;

    // Page elements
    private ILocator ResourceName => PageContext.Locator("h1");
    private ILocator EditResourceLink => PageContext.Locator("a").Filter(new() { HasText = "Edit resource" });
    private ILocator ResourceStatus => PageContext.Locator("strong");
    private ILocator ResourceType => PageContext.Locator("h2");
    private ILocator ResourceDetailsTable => PageContext.Locator("table").Nth(1); // Second table on the page

    public void SetResourceFormData(ResourceFormData resourceFormData)
    {
        _resourceFormData = resourceFormData;
    }

    // Actions
    public async Task AssertResourceName(string expectedResourceName)
    {
        await Expect(ResourceName).ToHaveTextAsync(expectedResourceName);
    }

    public async Task AssertResourceStatus(string expectedResourceStatus)
    {
        await Expect(ResourceStatus).ToHaveTextAsync(expectedResourceStatus);
    }

    public async Task AssertResourceType(string expectedResourceType)
    {
        await Expect(ResourceType).ToHaveTextAsync(expectedResourceType);
    }

    public async Task ValidateResourceStatusAgainstFormData()
    {
        if (_resourceFormData == null)
        {
            throw new InvalidOperationException("ResourceFormData has not been set.");
        }

        // Resource Status ID mapping
        var expectedStatus = _resourceFormData.ResourceStatusId switch
        {
            2 => "Active",
            _ => throw new NotSupportedException($"ResourceStatusId {_resourceFormData.ResourceStatusId} is not currently supported in the mapping.")
        };

        await AssertResourceStatus(expectedStatus);
    }

    public async Task ValidateResourceTypeAgainstFormData()
    {
        if (_resourceFormData == null)
        {
            throw new InvalidOperationException("ResourceFormData has not been set.");
        }

        // Resource Type ID mapping
        var expectedType = _resourceFormData.ResourceTypeId switch
        {
            1 => "Guidance details",
            2 => "Service details",
            _ => throw new NotSupportedException($"ResourceTypeId {_resourceFormData.ResourceTypeId} is not currently supported in the mapping.")
        };

        await AssertResourceType(expectedType);
    }

    public async Task ValidateResourceDetailsTableAgainstFormData(string linkedTeamName)
    {
        if (_resourceFormData == null)
        {
            throw new InvalidOperationException("ResourceFormData has not been set.");
        }

        var tableRows = ResourceDetailsTable.Locator("tbody tr");

        if (_resourceFormData.ResourceTypeId == 1)
        {
            // ResourceTypeId = 1: Guidance details (5 rows)
            await Expect(tableRows).ToHaveCountAsync(5);

            // Row 1: GuidanceName
            var guidanceNameCell = tableRows.Nth(0).Locator("td");
            await Expect(guidanceNameCell).ToHaveTextAsync(_resourceFormData.ResourceName);

            // Row 2: GuidanceLink
            var guidanceLinkCell = tableRows.Nth(1).Locator("td");
            await Expect(guidanceLinkCell).ToHaveTextAsync(_resourceFormData.DigitalLink);

            // Row 3: LinkedTeamName
            var linkedTeamNameCell = tableRows.Nth(2).Locator("td");
            ////await Expect(linkedTeamNameCell).ToHaveTextAsync(linkedTeamName); // TODO: Reinstate this when the front-end changes are made.

            // Row 4: GuidanceType
            var guidanceTypeCell = tableRows.Nth(3).Locator("td");
            
            // GuidanceTypeId mapping
            if (_resourceFormData.GuidanceTypeId.HasValue)
            {
                var expectedGuidanceType = _resourceFormData.GuidanceTypeId switch
                {
                    1 => "Statutory",
                    2 => "Non-Statutory",
                    _ => throw new NotSupportedException($"GuidanceTypeId {_resourceFormData.GuidanceTypeId} is not currently supported in the mapping.")
                };
                await Expect(guidanceTypeCell).ToHaveTextAsync(expectedGuidanceType);
            }
        }
        else if (_resourceFormData.ResourceTypeId == 2)
        {
            // ResourceTypeId = 2: Service details (3 rows)
            await Expect(tableRows).ToHaveCountAsync(3);

            // Row 1: ServiceName
            var serviceNameCell = tableRows.Nth(0).Locator("td");
            await Expect(serviceNameCell).ToHaveTextAsync(_resourceFormData.ResourceName);

            // Row 2: ServiceLink
            var serviceLinkCell = tableRows.Nth(1).Locator("td");
            await Expect(serviceLinkCell).ToHaveTextAsync(_resourceFormData.DigitalLink);

            // Row 3: LinkedTeamName
            var linkedTeamNameCell = tableRows.Nth(2).Locator("td");
            ////await Expect(linkedTeamNameCell).ToHaveTextAsync(linkedTeamName); // TODO: Reinstate this when the front-end changes are made.
        }
        else
        {
            throw new NotSupportedException($"ResourceTypeId {_resourceFormData.ResourceTypeId} is not currently supported for table validation.");
        }
    }
}