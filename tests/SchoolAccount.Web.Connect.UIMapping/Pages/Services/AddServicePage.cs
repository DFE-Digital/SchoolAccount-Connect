using Microsoft.Playwright;
using PlaywrightTests.DfE.UIMapping.Forms;
using PlaywrightTests.DfE.UIMapping.Mappers;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Services;

/// <summary>
/// Page model for the add service page.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
// TODO: This will need to be revisited as the UI is in a bizarre hybrid state of Service / Team.
public class AddServicePage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    // Page elements
    public ILocator ServiceName => PageContext.Locator("input[id='teamName']");
    public ILocator ServiceAcronym => PageContext.Locator("input[id='acronym']");
    public ILocator DigitalServiceLink => PageContext.Locator("input[id='digitalServiceLink']");
    public ILocator Description => PageContext.Locator("textarea[id='serviceDescription']");
    public ILocator DeputyDirector => PageContext.Locator("input[id='deputyDirector']");
    public ILocator TeamEmailAddress => PageContext.Locator("input[id='teamEmail']");
    public ILocator ServiceOwnerNames => PageContext.Locator("input[id='teamOwnerNames']");
    public ILocator ContactName => PageContext.Locator("input[id='fullName-0']");
    public ILocator ContactEmail => PageContext.Locator("input[id='email-0']");
    
    // Service Status Radio Buttons
    public ILocator ServiceStatusActive => PageContext.Locator("input[id='teamStatusOption-active']");
    public ILocator ServiceStatusDecommissioned => PageContext.Locator("input[id='teamStatusOption-decommissioned']");
    public ILocator ServiceStatusDueDecommissioned => PageContext.Locator("input[id='teamStatusOption-due']");
    
    // Decommissioning Date Fields
    public ILocator DecomDay => PageContext.Locator("input[id='dueToBeDecommissionedDate-day']");
    public ILocator DecomMonth => PageContext.Locator("input[id='dueToBeDecommissionedDate-month']");
    public ILocator DecomYear => PageContext.Locator("input[id='dueToBeDecommissionedDate-year']");
    
    // Hints
    public ILocator DecommissionedHint => PageContext.Locator("div[id='hintStatusOptopnDecom']");

    // Subgroup Mapping - Maps subgroup names to directorate IDs
    private static readonly Dictionary<string, Dictionary<string, string>> SubgroupMapping = SubgroupMappings.GroupSubgroupMapping;

    // Actions
    public async Task VerifyPageTitleAsync()
    {
        await AssertTitle("Add a service");
    }

    private ILocator GetGroupRadioButton(string groupName)
    {
        var groupId = groupName switch
        {
            "Education and Skills Funding Agency" => "group-0",
            "Families" => "group-1", 
            "Operations and Infrastructure" => "group-2",
            "Regions" => "group-3",
            "Schools" => "group-4",
            "Skills" => "group-5",
            "Strategy" => "group-6",
            _ => throw new ArgumentException($"Unknown group name: {groupName}. Valid options are: Education and Skills Funding Agency, Families, Operations and Infrastructure, Regions, Schools, Skills, Strategy")
        };
        RunContext.Log($"Mapping group name '{groupName}' to ID '{groupId}'");
        return PageContext.Locator($"input[id='{groupId}']");
    }

    public async Task ClickGroupRadioButtonAsync(string groupName)
    {
        await GetGroupRadioButton(groupName).ClickAsync();
    }

    private ILocator GetSupportLevelRadioButton(string supportLevel)
    {
        var levelId = supportLevel switch
        {
            "Passive" => "supportLevel-1",
            "Semi" => "supportLevel-2",
            "Active" => "supportLevel-3",
            _ => throw new ArgumentException($"Unknown support level: {supportLevel}. Valid options are: Passive, Semi, Active")
        };
        
        return PageContext.Locator($"input[id='{levelId}']");
    }

    public async Task ClickSupportLevelRadioButtonAsync(string supportLevel)
    {
        await GetSupportLevelRadioButton(supportLevel).ClickAsync();
    }

    /// Find a subgroup radio button
    public ILocator GetSubgroupRadioButton(string subgroupName)
    {
        // Find which group contains this subgroup
        foreach (var group in SubgroupMapping)
        {
            if (group.Value.ContainsKey(subgroupName))
            {
                var directorateId = group.Value[subgroupName];
                return PageContext.Locator($"input[id='{directorateId}']");
            }
        }

        throw new ArgumentException($"Unknown subgroup name: '{subgroupName}'. Please refer to SubgroupMappings.cs for available groups and subgroups.");
    }

    /// Lookup subgroup by name and click its radio button
    public async Task ClickSubgroupRadioButtonAsync(string subgroupName)
    {
        await GetSubgroupRadioButton(subgroupName).ClickAsync();
    }

    /// Choose a group and then select a random subgroup
    public async Task ClickGroupWithRandomSubgroupAsync(string groupName)
    {
        // First click the group
        await ClickGroupRadioButtonAsync(groupName);
        
        // Then automatically select a random subgroup from that group
        if (!SubgroupMapping.ContainsKey(groupName))
        {
            throw new ArgumentException($"Unknown group name: '{groupName}'. Valid groups are: {string.Join(", ", SubgroupMapping.Keys)}");
        }

        var faker = new Bogus.Faker();
        var availableSubgroups = SubgroupMapping[groupName].Keys.ToArray();
        var randomSubgroup = faker.PickRandom(availableSubgroups);
        
        await ClickSubgroupRadioButtonAsync(randomSubgroup);
    }

    public async Task SaveAndContinueAsync()
    {
        var saveAndContinueButton = PageContext.Locator("button").Filter(new() { HasText = "Save and continue" });
        await saveAndContinueButton.ClickAsync();
    }

    public async Task SaveAndFinishLaterAsync()
    {
        var saveAndFinishLaterButton = PageContext.Locator("button").Filter(new() { HasText = "Save and finish later" });
        await saveAndFinishLaterButton.ClickAsync();
    }

    public async Task<string> EnterUniqueServiceNameAsync(string baseName)
    {
        var uniqueName = $"{baseName}-{Guid.NewGuid()}";
        await ServiceName.FillAsync(uniqueName);
        return uniqueName;
    }

    /// Fills out the entire Add Service form using ServiceFormData
    /// Returns the service name that was filled in for verification purposes
    public async Task<string> FillFormWithServiceDataAsync(ServiceFormData serviceFormData)
    {
        // Fill basic service information
        await ServiceName.FillAsync(serviceFormData.ServiceName);
        await ServiceAcronym.FillAsync(serviceFormData.Acronym);
        await DigitalServiceLink.FillAsync(serviceFormData.DigitalServiceLink);
        await Description.FillAsync(serviceFormData.ServiceDescription);

        // Handle group/subgroup selection using IDs to get friendly names
        var groupName = ServiceFormData.GetGroupNameFromId(serviceFormData.GroupId);
        var subgroupName = ServiceFormData.GetSubgroupNameFromDirectorateId(serviceFormData.DirectorateId);
        await ClickGroupRadioButtonAsync(groupName);
        await ClickSubgroupRadioButtonAsync(subgroupName);
        
        await DeputyDirector.FillAsync(serviceFormData.DeputyDirector);
        await ServiceOwnerNames.FillAsync(serviceFormData.ServiceOwnerNames);
        
        // Use ServiceOwnerNames and TeamInboxEmail for contact fields (UI still needs these)
        await ContactName.FillAsync(serviceFormData.ServiceOwnerNames);
        await ContactEmail.FillAsync(serviceFormData.TeamInboxEmail);
        await TeamEmailAddress.FillAsync(serviceFormData.TeamInboxEmail);
        
        // Handle support level using ID to get friendly name
        var supportLevelName = ServiceFormData.GetSupportLevelName(serviceFormData.SupportLevelId);
        await ClickSupportLevelRadioButtonAsync(supportLevelName);
        
        // Handle service status selection with automatic date handling
        await SelectServiceStatusByIdAsync(serviceFormData.ServiceStatusId, serviceFormData.DueToDecommissionDate);
        
        return serviceFormData.ServiceName;
    }

    /// Handles service status selection based on database ServiceStatusId
    /// ServiceStatusId: 2 = Active, 3 = dueDecommissioned, 4 = Decommissioned
    public async Task SelectServiceStatusByIdAsync(int serviceStatusId, DateTime? dueToDecommissionDate = null)
    {
        switch (serviceStatusId)
        {
            case 2: // Active
                await ServiceStatusActive.ClickAsync();
                break;
                
            case 3: // dueDecommissioned
                await ServiceStatusDueDecommissioned.ClickAsync();

                // Fill decommission date if provided
                if (dueToDecommissionDate.HasValue)
                {
                    var decommissionDate = dueToDecommissionDate.Value;
                    await DecomDay.FillAsync(decommissionDate.Day.ToString());
                    await DecomMonth.FillAsync(decommissionDate.Month.ToString());
                    await DecomYear.FillAsync(decommissionDate.Year.ToString());
                }
                else
                {
                    throw new ArgumentException("Due to be decommissioned date must be provided when selecting 'dueDecommissioned' status.");
                }
                break;
                
            case 4: // Decommissioned
                await ServiceStatusDecommissioned.ClickAsync();
                break;
                
            default:
                // Default to Active if unknown status
                await ServiceStatusActive.ClickAsync();
                break;
        }
    }
}
