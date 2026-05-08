using System.Text.Json.Serialization;

namespace SchoolAccount.Integration.AcademiesApi.Models;

public class SmartData
{
    [JsonPropertyName("probabilityOfDeclining")]
    public string ProbabilityOfDeclining { get; set; } = string.Empty;

    [JsonPropertyName("probabilityOfStayingTheSame")]
    public string ProbabilityOfStayingTheSame { get; set; } = string.Empty;

    [JsonPropertyName("probabilityOfImproving")]
    public string ProbabilityOfImproving { get; set; } = string.Empty;

    [JsonPropertyName("predictedChangeInProgress8Score")]
    public string PredictedChangeInProgress8Score { get; set; } = string.Empty;

    [JsonPropertyName("predictedChanceOfChangeOccurring")]
    public string PredictedChanceOfChangeOccurring { get; set; } = string.Empty;

    [JsonPropertyName("totalNumberOfRisks")]
    public string TotalNumberOfRisks { get; set; } = string.Empty;

    [JsonPropertyName("totalRiskScore")]
    public string TotalRiskScore { get; set; } = string.Empty;

    [JsonPropertyName("riskRatingNum")]
    public string RiskRatingNum { get; set; } = string.Empty;
}