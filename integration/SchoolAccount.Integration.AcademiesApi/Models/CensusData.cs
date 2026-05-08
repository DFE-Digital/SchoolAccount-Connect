using System.Text.Json.Serialization;

namespace SchoolAccount.Integration.AcademiesApi.Models;

public class CensusData
{
    [JsonPropertyName("censusDate")]
    public string CensusDate { get; set; } = string.Empty;

    [JsonPropertyName("numberOfPupils")]
    public string NumberOfPupils { get; set; } = string.Empty;

    [JsonPropertyName("numberOfBoys")]
    public string NumberOfBoys { get; set; } = string.Empty;

    [JsonPropertyName("numberOfGirls")]
    public string NumberOfGirls { get; set; } = string.Empty;

    [JsonPropertyName("percentageSen")]
    public string PercentageSen { get; set; } = string.Empty;

    [JsonPropertyName("percentageFsm")]
    public string PercentageFsm { get; set; } = string.Empty;

    [JsonPropertyName("percentageEnglishNotFirstLanguage")]
    public string PercentageEnglishNotFirstLanguage { get; set; } = string.Empty;

    [JsonPropertyName("perceantageEnglishFirstLanguage")]
    public string PerceantageEnglishFirstLanguage { get; set; } = string.Empty;

    [JsonPropertyName("percentageFirstLanguageUnclassified")]
    public string PercentageFirstLanguageUnclassified { get; set; } = string.Empty;

    [JsonPropertyName("numberEligableForFSM")]
    public string NumberEligableForFSM { get; set; } = string.Empty;

    [JsonPropertyName("numberEligableForFSM6Years")]
    public string NumberEligableForFSM6Years { get; set; } = string.Empty;

    [JsonPropertyName("percentageEligableForFSM6Years")]
    public string PercentageEligableForFSM6Years { get; set; } = string.Empty;
}