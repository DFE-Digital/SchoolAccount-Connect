using Bogus;
using PlaywrightTests.DfE.UIMapping.Mappers;
using PlaywrightTests.GlobalConstants.Global;

namespace PlaywrightTests.DfE.UIMapping.Forms;

//This class contains the helper functions for manipulating ServiceFormData.
public partial class ServiceFormData : FormDataBase
{
    private ServiceFormData() { }

    // Mapping dictionaries for converting friendly names to database IDs
    // TODO: Make these enums with the correct values, e.g.:
    // SupportLevel { Passive = 1, Semi = 2, Active = 3 }
    private static readonly Dictionary<string, int> SupportLevelMapping = new()
    {
        { "Passive", 1 },
        { "Semi", 2 },
        { "Active", 3 }
    };

    private static readonly Dictionary<string, int> ServiceStatusMapping = new()
    {
        { "Active", 2 },
        { "Decommissioned", 4 },
        { "Due to be decommissioned", 3 }
    };

    private static readonly Dictionary<string, int> GroupIdMapping = new()
    {
        { "Education and Skills Funding Agency", 1 },
        { "Families", 2 },
        { "Operations and Infrastructure", 3 },
        { "Regions", 4 },
        { "Schools", 5 },
        { "Strategy", 7 },
        { "Skills", 6 }
    };

    // Helper methods for converting between friendly names and database IDs
    public static int GetSupportLevelId(string supportLevel)
    {
        return SupportLevelMapping.TryGetValue(supportLevel, out var id) ? id : 1; // Default to Passive
    }

    public static int GetServiceStatusId(string serviceStatus)
    {
        return ServiceStatusMapping.TryGetValue(serviceStatus, out var id) ? id : 2; // Default to Active
    }

    // Helper methods to convert from IDs back to friendly names (for UI integration)
    public static string GetSupportLevelName(int supportLevelId)
    {
        return SupportLevelMapping.FirstOrDefault(x => x.Value == supportLevelId).Key ?? "Passive";
    }

    public static string GetServiceStatusName(int serviceStatusId)
    {
        return ServiceStatusMapping.FirstOrDefault(x => x.Value == serviceStatusId).Key ?? "Active";
    }

    public static string GetGroupNameFromId(int groupId)
    {
        return GroupIdMapping.FirstOrDefault(x => x.Value == groupId).Key ?? "Education and Skills Funding Agency";
    }

    public static string GetSubgroupNameFromDirectorateId(int directorateId)
    {
        // Find the subgroup name by searching through SubgroupMappings for the matching directorate ID
        foreach (var groupKvp in SubgroupMappings.GroupSubgroupMapping)
        {
            foreach (var subgroupKvp in groupKvp.Value)
            {
                if (subgroupKvp.Value == $"directorate-{directorateId}")
                {
                    return subgroupKvp.Key;
                }
            }
        }
        return "Default Subgroup"; // Fallback
    }

    public static string GetFullSubgroupNameFromDirectorateId(int directorateId)
    {
        return SubgroupMappings.GetFullDirectorateName(directorateId);
    }

    // Get GroupId from group name
    private static int GetGroupIdFromName(string groupName)
    {
        if (GroupIdMapping.TryGetValue(groupName, out var groupId))
        {
            return groupId;
        }

        Console.WriteLine($"ERROR: Could not find GroupId for group '{groupName}'");
        throw new ArgumentException($"Invalid group name: {groupName}");
    }

    // Get DirectorateId from SubgroupMappings (extracts number from "directorate-69" format)
    private static int GetDirectorateIdFromName(string groupName, string subgroupName)
    {
        if (SubgroupMappings.GroupSubgroupMapping.TryGetValue(groupName, out var subgroups) &&
            subgroups.TryGetValue(subgroupName, out var directorateString))
        {
            // Extract number from "directorate-69" format
            if (directorateString.StartsWith("directorate-") &&
                int.TryParse(directorateString.Substring("directorate-".Length), out int directorateId))
            {
                return directorateId;
            }
        }

        Console.WriteLine($"ERROR: Could not find DirectorateId for group '{groupName}', subgroup '{subgroupName}'");
        throw new ArgumentException($"Invalid group/subgroup combination: {groupName}/{subgroupName}");
    }

    /// Instantiate the ServiceFormData by calling this function.
    public static ServiceFormData GenerateRandomData(RandomGeneratorOptions options = RandomGeneratorOptions.None)
    {
        var sfData = new ServiceFormData();

        // Generate database-mapped fields
        sfData.ServiceName = $"{_faker.Company.CompanyName()}-{Guid.NewGuid().ToString()[..8]}";

        sfData.Acronym = _faker.Random.AlphaNumeric(3).ToUpper();

        sfData.DigitalServiceLink = _faker.Internet.Url();

        sfData.ServiceDescription = _faker.Lorem.Paragraph();

        sfData.DeputyDirector = _faker.Name.FullName();

        sfData.TeamInboxEmail = Global.TestEmailAddress;

        sfData.ServiceOwnerNames = _faker.Name.FullName();

        sfData.CreatedBy = Global.TestEmailAddress;

        sfData.UpdatedBy = Global.TestEmailAddress;

        // Set GroupId and DirectorateId using SubgroupMappings
        // Pick a random group name first
        var availableGroups = SubgroupMappings.GroupSubgroupMapping.Keys.ToArray();
        var selectedGroupName = _faker.PickRandom(availableGroups);

        // Pick a random subgroup from the selected group
        if (SubgroupMappings.GroupSubgroupMapping.ContainsKey(selectedGroupName))
        {
            var subgroupsForGroup = SubgroupMappings.GroupSubgroupMapping[selectedGroupName].Keys.ToArray();
            var selectedSubgroupName = _faker.PickRandom(subgroupsForGroup);

            // Set the IDs (we'll need to create lookup methods later)
            // For now, use placeholder values based on group/subgroup selection
            sfData.GroupId = GetGroupIdFromName(selectedGroupName);
            sfData.DirectorateId = GetDirectorateIdFromName(selectedGroupName, selectedSubgroupName);
        }

        // Set SupportLevelId using friendly name mapping

        var selectedSupportLevel = _faker.PickRandom(SupportLevelMapping.Keys.ToArray());
        sfData.SupportLevelId = GetSupportLevelId(selectedSupportLevel);

        // Set ServiceStatusId using friendly name mapping
        var serviceStatuses = ServiceStatusMapping.Keys.ToArray();
        if (options.HasFlag(RandomGeneratorOptions.NonDecommissionedOnly))
        {
            serviceStatuses = serviceStatuses.Where(ss => ss != "Decommissioned").ToArray();
        }

        var selectedServiceStatus = _faker.PickRandom(serviceStatuses);
        sfData.ServiceStatusId = GetServiceStatusId(selectedServiceStatus);

        // Generate decommissioning date if status is dueDecommissioned (ServiceStatusId = 3)
        if (sfData.ServiceStatusId == 3 && sfData.DueToDecommissionDate == null)
        {
            // Generate a future decommission date
            sfData.DueToDecommissionDate = new DateTime(2045, 1, 1);
            sfData.IsExactDecommissionDate = true;
        }

        // Handle decommissioned services (ServiceStatusId = 4)
        if (sfData.ServiceStatusId == 4)
        {
            sfData.DueToDecommissionDate = null;
            sfData.IsExactDecommissionDate = false;
        }

        return sfData;
    }
}