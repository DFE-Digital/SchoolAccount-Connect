namespace PlaywrightTests.DfE.UIMapping.Mappers;

public static class SubgroupMappings
{
    public static readonly Dictionary<string, Dictionary<string, string>> GroupSubgroupMapping = new()
    {
        ["Education and Skills Funding Agency"] = new Dictionary<string, string>
        {
            ["Finance"] = "directorate-1",
            ["Funding"] = "directorate-2",
        },
        ["Families"] = new Dictionary<string, string>
        {
            ["EarlyYears"] = "directorate-3",
            ["HelpSocial"] = "directorate-5",
            ["Vuln"] = "directorate-6",
            ["Strategy"] = "directorate-4"
        },
        ["Operations and Infrastructure"] = new Dictionary<string, string>
        {
            ["Construction"] = "directorate-11",
            ["EstatesNZ"] = "directorate-12",
            ["Digital"] = "directorate-13",
            ["CommercialSector"] = "directorate-7",
            ["Tech"] = "directorate-14",
            ["HR"] = "directorate-8",
            ["OpFinance"] = "directorate-9",
            ["StratFinance"] = "directorate-10"
        },
        ["Regions"] = new Dictionary<string, string>
        {
            ["SchoolFinSupp"] = "directorate-15",
            ["NationalDirectorate"] = "directorate-16",
            ["Territories"] = "directorate-17"
        },
        ["Schools"] = new Dictionary<string, string>
        {
            ["Curriculum"] = "directorate-18",
            ["SchoolSystem"] = "directorate-23",
            ["Standards"] = "directorate-19",
            ["CTSPA"] = "directorate-21",
            ["SEND"] = "directorate-22",
            ["Wellbeing"] = "directorate-20",
            ["TeachersTutors"] = "directorate-24"

        },
        ["Skills"] = new Dictionary<string, string>
        {
            ["SkillStrat"] = "directorate-25",
            ["Apprenticeships"] = "directorate-26",
            ["HEOversight"] = "directorate-27",
            ["FEOversight"] = "directorate-28",
            ["TechSkills"] = "directorate-29",
            ["Regions"] = "directorate-30",
            ["SkillsJourney"] = "directorate-31",
        },
        ["Strategy"] = new Dictionary<string, string>
        {
            ["Communications"] = "directorate-32",
            ["Resilience"] = "directorate-33",
            ["StratAnalysis"] = "directorate-34",
            ["StratDelivery"] = "directorate-35",
            ["Legal"] = "directorate-36",
            ["StratPolicy"] = "directorate-37"
        }
    };

    private static readonly Dictionary<int, string> DirectorateFullNameMapping = new()
    {
        { 1, "Finance and Provider Market Oversight" },
        { 2, "Funding" },
        { 3, "Early Years, Childcare, Families and Analysis" },
        { 4, "Strategy and Care System" },
        { 5, "Help, Protection and Social Work" },
        { 6, "Vulnerable Children Strategy and Educational Engagement" },
        { 7, "Commercial for Sector and Commercial Operations" },
        { 8, "HR and Transformation" },
        { 9, "Operational Finance" },
        { 10, "Strategic Finance" },
        { 11, "Construction Delivery Education Estates" },
        { 12, "Education Estates and Net Zero" },
        { 13, "Digital" },
        { 14, "Technology" },
        { 15, "Schools Financial Support and Oversight" },
        { 16, "National Directorate" },
        { 17, "Territories" },
        { 18, "Curriculum and General Qualifications" },
        { 19, "Standards and Testing Agency" },
        { 20, "Pupil Wellbeing and Safety" },
        { 21, "Candidates, Trainees, Strategy, Portfolio and Analysis" },
        { 22, "SEND and Alternative Provision" },
        { 23, "School Analysis, System and Strategy" },
        { 24, "Teachers and Tutors" },
        { 25, "Skills Strategy and Engagement" },
        { 26, "Apprenticeships and Skills Bootcamps" },
        { 27, "Higher Education Oversight" },
        { 28, "Further Education Oversight" },
        { 29, "Technical Qualifications and Essential Skills" },
        { 30, "Regions and Providers" },
        { 31, "Skills Journey" },
        { 32, "Communications" },
        { 33, "Resilience" },
        { 34, "Strategic Analysis, Research, Data, Intelligence, and Science" },
        { 35, "Strategy and Delivery" },
        { 36, "Legal Advisers Office" },
        { 37, "Strategic Policy" }
    };

    // Helper method to get directorate ID from the string value (e.g., "directorate-3" -> 3)
    public static int GetDirectorateId(string directorateValue)
    {
        if (string.IsNullOrEmpty(directorateValue))
            return 1;

        var parts = directorateValue.Split('-');
        if (parts.Length == 2 && int.TryParse(parts[1], out int id))
            return id;

        return 1; // Default
    }

    // Get directorates for a specific group
    public static Dictionary<string, int> GetDirectoratesForGroup(string groupName)
    {
        if (!GroupSubgroupMapping.ContainsKey(groupName))
            return new Dictionary<string, int>();

        var directorates = GroupSubgroupMapping[groupName];
        return directorates.ToDictionary(
            kvp => kvp.Key,
            kvp => GetDirectorateId(kvp.Value)
        );
    }

    // Get all group names
    public static List<string> GetAllGroups()
    {
        return GroupSubgroupMapping.Keys.ToList();
    }

    public static string GetFullDirectorateName(int directorateId)
    {
        return DirectorateFullNameMapping.ContainsKey(directorateId)
            ? DirectorateFullNameMapping[directorateId]
            : "Unknown Directorate";
    }
}