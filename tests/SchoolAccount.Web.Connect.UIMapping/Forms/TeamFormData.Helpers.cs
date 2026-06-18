using Bogus;
using PlaywrightTests.DfE.UIMapping.Mappers;
using PlaywrightTests.GlobalConstants.Global;

namespace PlaywrightTests.DfE.UIMapping.Forms;

//This class contains the helper functions for manipulating ServiceFormData.
public partial class TeamFormData : FormDataBase
{
    private TeamFormData() { }

    // Mapping dictionaries for converting friendly names to database IDs
    // TODO: Make these enums with the correct values, e.g.:
    // SupportLevel { Passive = 1, Semi = 2, Active = 3 }
    private static readonly Dictionary<string, int> SupportLevelMapping = new()
    {
        { "Passive", 1 },
        { "Semi", 2 },
        { "Active", 3 }
    };

    private static readonly Dictionary<string, int> TeamStatusMapping = new()
    {
        // Note that Draft is excluded.
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

    private static readonly Dictionary<string, int> WorkflowStateMapping = new()
    {
        // Note that Draft and Queued are excluded.
        { "Published", 3 },
        { "Expired", 4 },
        { "Archived", 5 }
    };

    // Helper methods for converting between friendly names and database IDs
    public static int GetSupportLevelId(string supportLevel)
    {
        return SupportLevelMapping.TryGetValue(supportLevel, out var id) ? id : 1; // Default to Passive
    }

    public static int GetTeamStatusId(string teamStatus)
    {
        return TeamStatusMapping.TryGetValue(teamStatus, out var id) ? id : 2; // Default to Active
    }

    // Helper methods to convert from IDs back to friendly names (for UI integration)
    public static string GetSupportLevelName(int supportLevelId)
    {
        return SupportLevelMapping.FirstOrDefault(x => x.Value == supportLevelId).Key ?? "Passive";
    }

    public static string GetTeamStatusName(int teamStatusId)
    {
        return TeamStatusMapping.FirstOrDefault(x => x.Value == teamStatusId).Key ?? "Active";
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

    public static TeamFormData GenerateRandomData(RandomGeneratorOptions options = RandomGeneratorOptions.None)
    {
        var teamFormData = new TeamFormData();

        // Generate database-mapped fields
        teamFormData.TeamName = $"{_faker.Company.CompanyName()}-{Guid.NewGuid().ToString()[..8]}";
        teamFormData.Acronym = _faker.Random.AlphaNumeric(3).ToUpper();
        teamFormData.TeamDescription = _faker.Lorem.Paragraph();
        teamFormData.DeputyDirector = _faker.Name.FullName();
        teamFormData.TeamInboxEmail = Global.TestEmailAddress;
        teamFormData.TeamOwnerNames = _faker.Name.FullName();
        teamFormData.WorkflowStateId = _faker.PickRandom(WorkflowStateMapping.Values.ToArray());
        teamFormData.CreatedBy = Global.TestEmailAddress;
        teamFormData.UpdatedBy = Global.TestEmailAddress;
        teamFormData.DigitalServiceLink = _faker.Internet.Url();
        teamFormData.SupportLevelId = _faker.PickRandom(SupportLevelMapping.Values.ToArray());

        SetGroupAndDirectorate(teamFormData);
        SetTeamStatus(options, teamFormData);
        SetDecommissionFields(teamFormData);

        return teamFormData;
    }

    private static void SetGroupAndDirectorate(TeamFormData teamFormData)
    {
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
            teamFormData.GroupId = GetGroupIdFromName(selectedGroupName);
            teamFormData.DirectorateId = GetDirectorateIdFromName(selectedGroupName, selectedSubgroupName);
        }
    }

    private static void SetTeamStatus(RandomGeneratorOptions options, TeamFormData teamFormData)
    {
        var teamStatuses = TeamStatusMapping.Keys.ToArray();
        if (options.HasFlag(RandomGeneratorOptions.NonDecommissionedOnly))
        {
            teamStatuses = teamStatuses.Where(ss => ss != "Decommissioned").ToArray();
        }

        var selectedTeamStatus = _faker.PickRandom(teamStatuses);
        teamFormData.TeamStatusId = GetTeamStatusId(selectedTeamStatus);
    }

    private static void SetDecommissionFields(TeamFormData teamFormData)
    {
        // Generate decommissioning date if status is Due to be decommissioned
        if (teamFormData.TeamStatusId == TeamStatusMapping["Due to be decommissioned"] && teamFormData.DueToDecommissionDate == null)
        {
            // Generate a future decommission date
            teamFormData.DueToDecommissionDate = new DateTime(2045, 1, 1);
            teamFormData.IsExactDecommissionDate = true;
        }

        // Handle decommissioned services
        if (teamFormData.TeamStatusId == TeamStatusMapping["Decommissioned"])
        {
            teamFormData.DueToDecommissionDate = null;
            teamFormData.IsExactDecommissionDate = false;
        }
    }
}