using Bogus;

namespace SchoolAccount.Tests.Common.DataSets;

public static class SchoolDataSet
{
    public static readonly string[] SchoolTypes =
    [
        "Primary School",
        "High School",
        "Academy",
        "Junior School",
        "Community School",
        "Church of England Primary School",
        "Catholic Primary School",
        "Grammar School",
        "Free School",
        "Foundation School",
    ];

    public static readonly Dictionary<string, (int Low, int High)> SchoolAgeRanges = new()
    {
        { "Primary School", (4, 11) },
        { "High School", (11, 18) },
        { "Academy", (11, 18) },
        { "Junior School", (7, 11) },
        { "Community School", (11, 16) },
        { "Church of England Primary School", (4, 11) },
        { "Catholic Primary School", (4, 11) },
        { "Grammar School", (11, 18) },
        { "Free School", (4, 16) },
        { "Foundation School", (11, 16) },
    };

    public static string GetSchoolName(this Faker faker)
    {
        return faker.GetSchoolName(out _, out _);
    }

    public static string GetSchoolName(this Faker faker, out int lowAge, out int highAge)
    {
        var schoolPrefixes = new[] { faker.Address.City(), faker.Address.StreetName(), faker.Name.LastName() };

        var schoolType = faker.PickRandom(SchoolTypes);
        lowAge = SchoolAgeRanges[schoolType].Low;
        highAge = SchoolAgeRanges[schoolType].High;
        return $"{faker.PickRandom(schoolPrefixes)} {schoolType}";
    }
}
