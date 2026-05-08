using System.Text.Json.Serialization;

namespace SchoolAccount.Integration.AcademiesApi.Models;

public class TrustGiasData
{
    [JsonPropertyName("groupId")]
    public string? GroupId { get; set; }

    [JsonPropertyName("groupName")]
    public string? GroupName { get; set; }

    [JsonPropertyName("groupType")]
    public string? GroupType { get; set; }

    [JsonPropertyName("companiesHouseNumber")]
    public string? CompaniesHouseNumber { get; set; }

    [JsonPropertyName("groupContactAddress")]
    public AddressData? GroupContactAddress { get; set; }

    [JsonPropertyName("ukprn")]
    public string? Ukprn { get; set; }
}