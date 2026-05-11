using Bogus;
using SchoolAccount.Integration.DfESignIn.Common;

namespace SchoolAccount.Tests.Common.DataSets;

public static class LocalAuthorityDataSet
{
    public static readonly string[] AuthoritiesTypes =
    [
        "City Council",
        "County Council",
        "Metropolitan Borough Council",
        "Metropolitan District Council",
        "Council"
    ];

    public static string GetAuthorityName(this Faker faker, out string city)
    {
        city = faker.Address.City();
        return $"{city} {faker.PickRandom(AuthoritiesTypes)}";
    }

    public static string ToShortAuthorityName(this string name)
    {
        return AuthoritiesTypes
            .Aggregate(name, (current, authority) =>
                current.Replace(authority, string.Empty, StringComparison.InvariantCultureIgnoreCase))
            .Trim();
    }

    /// <summary>
    /// Generate a GSS/ONS Local Authority Name
    /// </summary>
    /// <remarks>See https://get-information-schools.service.gov.uk/Guidance/LaNameCodes for what this is trying to emulate</remarks>
    public static string GenerateGssCode(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return string.Empty;
        }
        
        var typeCode = "07";
        
        if (name.Contains("Metropolitan", StringComparison.InvariantCultureIgnoreCase))
        {
            typeCode = "08";
        }
        else if (name.Contains("City", StringComparison.InvariantCultureIgnoreCase))
        {
            typeCode = "06";
        }
        else if (name.Contains("County", StringComparison.InvariantCultureIgnoreCase))
        {
            typeCode = "10";
        }

        var hash = Math.Abs(name.GetHashCode(StringComparison.InvariantCultureIgnoreCase)) % 999999;

        return $"E{typeCode}{hash:D6}";
    }
}