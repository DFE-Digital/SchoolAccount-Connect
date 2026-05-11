using System.Text.Json.Serialization;

namespace SchoolAccount.Integration.AcademiesApi.Models;

public class ViewAcademyConversionData
{
    [JsonPropertyName("viabilityIssue")]
    public string? ViabilityIssue { get; set; }

    [JsonPropertyName("pfi")]
    public string? Pfi { get; set; }

    [JsonPropertyName("pan")]
    public string? Pan { get; set; }

    [JsonPropertyName("deficit")]
    public string? Deficit { get; set; }
}