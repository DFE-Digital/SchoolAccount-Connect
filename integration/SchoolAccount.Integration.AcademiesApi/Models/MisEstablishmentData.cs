using System.Text.Json.Serialization;

namespace SchoolAccount.Integration.AcademiesApi.Models;

public class MisEstablishmentData
{
    [JsonPropertyName("siteName")]
    public string? SiteName { get; set; }

    [JsonPropertyName("webLink")]
    public string WebLink { get; set; } = string.Empty;

    [JsonPropertyName("laestab")]
    public string Laestab { get; set; } = string.Empty;

    [JsonPropertyName("schoolName")]
    public string SchoolName { get; set; } = string.Empty;

    [JsonPropertyName("ofstedPhase")]
    public string OfstedPhase { get; set; } = string.Empty;

    [JsonPropertyName("typeOfEducation")]
    public string TypeOfEducation { get; set; } = string.Empty;

    [JsonPropertyName("schoolOpenDate")]
    public string? SchoolOpenDate { get; set; }

    [JsonPropertyName("sixthForm")]
    public string SixthForm { get; set; } = string.Empty;

    [JsonPropertyName("designatedReligiousCharacter")]
    public string DesignatedReligiousCharacter { get; set; } = string.Empty;

    [JsonPropertyName("religiousEthos")]
    public string ReligiousEthos { get; set; } = string.Empty;

    [JsonPropertyName("faithGrouping")]
    public string FaithGrouping { get; set; } = string.Empty;

    [JsonPropertyName("ofstedRegion")]
    public string OfstedRegion { get; set; } = string.Empty;

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

    [JsonPropertyName("localAuthority")]
    public string LocalAuthority { get; set; } = string.Empty;

    [JsonPropertyName("parliamentaryConstituency")]
    public string ParliamentaryConstituency { get; set; } = string.Empty;

    [JsonPropertyName("postcode")]
    public string Postcode { get; set; } = string.Empty;

    [JsonPropertyName("incomeDeprivationAffectingChildrenIndexQuintile")]
    public string IncomeDeprivationAffectingChildrenIndexQuintile { get; set; } = string.Empty;

    [JsonPropertyName("totalNumberOfPupils")]
    public string TotalNumberOfPupils { get; set; } = string.Empty;

    [JsonPropertyName("latestSection8InspectionNumberSinceLastFullInspection")]
    public string? LatestSection8InspectionNumberSinceLastFullInspection { get; set; }

    [JsonPropertyName("section8InspectionRelatedToCurrentSchoolUrn")]
    public string? Section8InspectionRelatedToCurrentSchoolUrn { get; set; }

    [JsonPropertyName("urnAtTimeOfSection8Inspection")]
    public string UrnAtTimeOfSection8Inspection { get; set; } = string.Empty;

    [JsonPropertyName("schoolNameAtTimeOfSection8Inspection")]
    public string? SchoolNameAtTimeOfSection8Inspection { get; set; }

    [JsonPropertyName("schoolTypeAtTimeOfSection8Inspection")]
    public string? SchoolTypeAtTimeOfSection8Inspection { get; set; }

    [JsonPropertyName("numberOfSection8InspectionsSinceLastFullInspection")]
    public string NumberOfSection8InspectionsSinceLastFullInspection { get; set; } = string.Empty;

    [JsonPropertyName("dateOfLatestSection8Inspection")]
    public string? DateOfLatestSection8Inspection { get; set; }

    [JsonPropertyName("section8InspectionPublicationDate")]
    public string? Section8InspectionPublicationDate { get; set; }

    [JsonPropertyName("latestSection8InspectionConvertedToFullInspection")]
    public string? LatestSection8InspectionConvertedToFullInspection { get; set; }

    [JsonPropertyName("section8InspectionOverallOutcome")]
    public string? Section8InspectionOverallOutcome { get; set; }

    [JsonPropertyName("inspectionNumberOfLatestFullInspection")]
    public string InspectionNumberOfLatestFullInspection { get; set; } = string.Empty;

    [JsonPropertyName("inspectionType")]
    public string InspectionType { get; set; } = string.Empty;

    [JsonPropertyName("inspectionTypeGrouping")]
    public string InspectionTypeGrouping { get; set; } = string.Empty;

    [JsonPropertyName("inspectionStartDate")]
    public string InspectionStartDate { get; set; } = string.Empty;

    [JsonPropertyName("inspectionEndDate")]
    public string? InspectionEndDate { get; set; }

    [JsonPropertyName("publicationDate")]
    public string PublicationDate { get; set; } = string.Empty;

    [JsonPropertyName("latestFullInspectionRelatesToCurrentSchoolUrn")]
    public string LatestFullInspectionRelatesToCurrentSchoolUrn { get; set; } = string.Empty;

    [JsonPropertyName("schoolUrnAtTimeOfLastFullInspection")]
    public string SchoolUrnAtTimeOfLastFullInspection { get; set; } = string.Empty;

    [JsonPropertyName("laestabAtTimeOfLastFullInspection")]
    public string LaestabAtTimeOfLastFullInspection { get; set; } = string.Empty;

    [JsonPropertyName("schoolNameAtTimeOfLastFullInspection")]
    public string SchoolNameAtTimeOfLastFullInspection { get; set; } = string.Empty;

    [JsonPropertyName("schoolTypeAtTimeOfLastFullInspection")]
    public string SchoolTypeAtTimeOfLastFullInspection { get; set; } = string.Empty;

    [JsonPropertyName("overallEffectiveness")]
    public string OverallEffectiveness { get; set; } = string.Empty;

    [JsonPropertyName("categoryOfConcern")]
    public string? CategoryOfConcern { get; set; }

    [JsonPropertyName("qualityOfEducation")]
    public string QualityOfEducation { get; set; } = string.Empty;

    [JsonPropertyName("behaviourAndAttitudes")]
    public string BehaviourAndAttitudes { get; set; } = string.Empty;

    [JsonPropertyName("personalDevelopment")]
    public string PersonalDevelopment { get; set; } = string.Empty;

    [JsonPropertyName("effectivenessOfLeadershipAndManagement")]
    public string EffectivenessOfLeadershipAndManagement { get; set; } = string.Empty;

    [JsonPropertyName("safeguardingIsEffective")]
    public string SafeguardingIsEffective { get; set; } = string.Empty;

    [JsonPropertyName("earlyYearsProvision")]
    public string EarlyYearsProvision { get; set; } = string.Empty;

    [JsonPropertyName("sixthFormProvision")]
    public string SixthFormProvision { get; set; } = string.Empty;

    [JsonPropertyName("previousFullInspectionNumber")]
    public string PreviousFullInspectionNumber { get; set; } = string.Empty;

    [JsonPropertyName("previousInspectionStartDate")]
    public string PreviousInspectionStartDate { get; set; } = string.Empty;

    [JsonPropertyName("previousInspectionEndDate")]
    public string? PreviousInspectionEndDate { get; set; }

    [JsonPropertyName("previousPublicationDate")]
    public string PreviousPublicationDate { get; set; } = string.Empty;

    [JsonPropertyName("previousFullInspectionRelatesToUrnOfCurrentSchool")]
    public string PreviousFullInspectionRelatesToUrnOfCurrentSchool { get; set; } = string.Empty;

    [JsonPropertyName("urnAtTheTimeOfPreviousFullInspection")]
    public string UrnAtTheTimeOfPreviousFullInspection { get; set; } = string.Empty;

    [JsonPropertyName("laestabAtTheTimeOfPreviousFullInspection")]
    public string LaestabAtTheTimeOfPreviousFullInspection { get; set; } = string.Empty;

    [JsonPropertyName("schoolNameAtTheTimeOfPreviousFullInspection")]
    public string SchoolNameAtTheTimeOfPreviousFullInspection { get; set; } = string.Empty;

    [JsonPropertyName("schoolTypeAtTheTimeOfPreviousFullInspection")]
    public string SchoolTypeAtTheTimeOfPreviousFullInspection { get; set; } = string.Empty;

    [JsonPropertyName("previousFullInspectionOverallEffectiveness")]
    public string PreviousFullInspectionOverallEffectiveness { get; set; } = string.Empty;

    [JsonPropertyName("previousCategoryOfConcern")]
    public string? PreviousCategoryOfConcern { get; set; }

    [JsonPropertyName("previousQualityOfEducation")]
    public string PreviousQualityOfEducation { get; set; } = string.Empty;

    [JsonPropertyName("previousBehaviourAndAttitudes")]
    public string PreviousBehaviourAndAttitudes { get; set; } = string.Empty;

    [JsonPropertyName("previousPersonalDevelopment")]
    public string PreviousPersonalDevelopment { get; set; } = string.Empty;

    [JsonPropertyName("previousEffectivenessOfLeadershipAndManagement")]
    public string PreviousEffectivenessOfLeadershipAndManagement { get; set; } = string.Empty;

    [JsonPropertyName("previousIsSafeguardingEffective")]
    public string PreviousIsSafeguardingEffective { get; set; } = string.Empty;

    [JsonPropertyName("previousEarlyYearsProvision")]
    public string PreviousEarlyYearsProvision { get; set; } = string.Empty;

    [JsonPropertyName("previousSixthFormProvision")]
    public string PreviousSixthFormProvision { get; set; } = string.Empty;
}