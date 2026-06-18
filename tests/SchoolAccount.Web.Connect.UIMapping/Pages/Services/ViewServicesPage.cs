using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;
using static Microsoft.Playwright.Assertions;
using PlaywrightTests.DfE.UIMapping.Forms;

namespace PlaywrightTests.DfE.UIMapping.Pages.Services;

/// Page Object Model for the Services page
public class ViewServicesPage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    // Page elements
    public ILocator ServiceName => PageContext.Locator("h1[class='govuk-heading-l']");
    public ILocator EditServiceButton => PageContext.Locator("a").Filter(new() { HasText = "Edit service" });
    public ILocator ViewTasksButton => PageContext.Locator("a").Filter(new() { HasText = "View tasks and subtasks" });
    public ILocator GetServiceStatus => PageContext.Locator("strong");

    // GovUK Services List elements
    public ILocator ServicesList => PageContext.Locator("dl.govuk-summary-list");
    public ILocator ServicesListRows => ServicesList.Locator("div.govuk-summary-list__row");

    // Actions
    public async Task ValidateServiceNameAsync(string expectedServiceName)
    {
        await Expect(ServiceName).ToHaveTextAsync(expectedServiceName);
    }

    public async Task ClickEditServiceAsync()
    {
        await EditServiceButton.ClickAsync();
    }

    public async Task ValidateServiceStatusAsync(string expectedStatus)
    {
        await Expect(GetServiceStatus).ToHaveTextAsync(expectedStatus);
    }

    /// <summary>
    /// Asserts that the services list contains the expected value for the given key
    /// </summary>
    public async Task AssertServicesListValueByKeyAsync(string keyText, string expectedValue)
    {
        var rowCount = await ServicesListRows.CountAsync();

        for (int i = 0; i < rowCount; i++)
        {
            var row = ServicesListRows.Nth(i);
            var key = row.Locator("dt.govuk-summary-list__key");
            var keyContent = await key.TextContentAsync();

            if (keyContent != null && keyContent.Trim().Equals(keyText, StringComparison.OrdinalIgnoreCase))
            {
                var value = row.Locator("dd.govuk-summary-list__value");
                await Expect(value).ToContainTextAsync(expectedValue);
                return;
            }
        }

        throw new InvalidOperationException($"Services list key '{keyText}' not found");
    }

    /// <summary>
    /// Accepts a ServiceFormData object and validates the data in the service details table
    /// </summary>
    public async Task ValidateAllServiceDetailsAsync(ServiceFormData expectedData)
    {
        await AssertServicesListValueByKeyAsync("Service name", expectedData.ServiceName);
        await AssertServicesListValueByKeyAsync("Service acronym", expectedData.Acronym);
        // The digital service link doesn't currently work.
        // await AssertServicesListValueByKeyAsync("Digital service link", expectedData.DigitalServiceLink);
        await AssertServicesListValueByKeyAsync("Description", expectedData.ServiceDescription);

        // Map GroupId to friendly name for validation
        var groupName = ServiceFormData.GetGroupNameFromId(expectedData.GroupId);
        await AssertServicesListValueByKeyAsync("Group", groupName);

        // Map DirectorateId to friendly name for validation
        string directorateName = ServiceFormData.GetFullSubgroupNameFromDirectorateId(expectedData.DirectorateId);
        await AssertServicesListValueByKeyAsync("Directorate", directorateName);

        await AssertServicesListValueByKeyAsync("Deputy director", expectedData.DeputyDirector);
        await AssertServicesListValueByKeyAsync("Team email", expectedData.TeamInboxEmail);
        await AssertServicesListValueByKeyAsync("Service owner names", expectedData.ServiceOwnerNames);

        // Map SupportLevelId to friendly name for validation
        var supportLevel = ServiceFormData.GetSupportLevelName(expectedData.SupportLevelId);
        await AssertServicesListValueByKeyAsync("Support level", supportLevel);

        // Map ServiceStatusId to friendly name for validation
        var serviceStatus = ServiceFormData.GetServiceStatusName(expectedData.ServiceStatusId);
        await AssertServicesListValueByKeyAsync("Service status", serviceStatus);
    }
}