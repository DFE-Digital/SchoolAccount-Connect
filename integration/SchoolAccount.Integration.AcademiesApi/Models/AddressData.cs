using System.Text.Json.Serialization;

namespace SchoolAccount.Integration.AcademiesApi.Models;

public class AddressData
{
    [JsonPropertyName("street")]
    public string Street { get; set; } = string.Empty;

    [JsonPropertyName("locality")]
    public string Locality { get; set; } = string.Empty;

    [JsonPropertyName("additionalLine")]
    public string? AdditionalLine { get; set; }

    [JsonPropertyName("town")]
    public string Town { get; set; } = string.Empty;

    [JsonPropertyName("county")]
    public string? County { get; set; }

    [JsonPropertyName("postcode")]
    public string Postcode { get; set; } = string.Empty;
    
    override public string ToString() => $"{Street}, {Locality}, {AdditionalLine}, {County}, {Postcode}";
}