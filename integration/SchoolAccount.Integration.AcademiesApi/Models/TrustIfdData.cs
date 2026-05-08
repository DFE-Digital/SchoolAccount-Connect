using System.Text.Json.Serialization;

namespace SchoolAccount.Integration.AcademiesApi.Models;

public class TrustIfdData
{
    [JsonPropertyName("trustOpenDate")]
    public string? TrustOpenDate { get; set; }

    [JsonPropertyName("leadRSCRegion")]
    public string? LeadRSCRegion { get; set; }

    [JsonPropertyName("trustContactPhoneNumber")]
    public string? TrustContactPhoneNumber { get; set; }

    [JsonPropertyName("performanceAndRiskDateOfMeeting")]
    public string? PerformanceAndRiskDateOfMeeting { get; set; }

    [JsonPropertyName("prioritisedAreaOfReview")]
    public string? PrioritisedAreaOfReview { get; set; }

    [JsonPropertyName("currentSingleListGrouping")]
    public string? CurrentSingleListGrouping { get; set; }

    [JsonPropertyName("dateOfGroupingDecision")]
    public string? DateOfGroupingDecision { get; set; }

    [JsonPropertyName("dateEnteredOntoSingleList")]
    public string? DateEnteredOntoSingleList { get; set; }

    [JsonPropertyName("trustReviewWriteup")]
    public string? TrustReviewWriteup { get; set; }

    [JsonPropertyName("dateOfTrustReviewMeeting")]
    public string? DateOfTrustReviewMeeting { get; set; }

    [JsonPropertyName("followupLetterSent")]
    public string? FollowupLetterSent { get; set; }

    [JsonPropertyName("dateActionPlannedFor")]
    public string? DateActionPlannedFor { get; set; }

    [JsonPropertyName("wipSummaryGoesToMinister")]
    public string? WipSummaryGoesToMinister { get; set; }

    [JsonPropertyName("externalGovernanceReviewDate")]
    public string? ExternalGovernanceReviewDate { get; set; }

    [JsonPropertyName("efficiencyICFPreviewCompleted")]
    public string? EfficiencyICFPreviewCompleted { get; set; }

    [JsonPropertyName("efficiencyICFPreviewOther")]
    public string? EfficiencyICFPreviewOther { get; set; }

    [JsonPropertyName("linkToWorkplaceForEfficiencyICFReview")]
    public string? LinkToWorkplaceForEfficiencyICFReview { get; set; }

    [JsonPropertyName("numberInTrust")]
    public string? NumberInTrust { get; set; }

    [JsonPropertyName("trustType")]
    public string? TrustType { get; set; }

    [JsonPropertyName("trustAddress")]
    public AddressData? TrustAddress { get; set; }
}