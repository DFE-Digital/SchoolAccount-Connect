namespace SchoolAccount.Integration.DfESignIn;

/// <summary>
/// Describes what type of organisation the establishment is.
/// </summary>
/// <remarks>
/// Go to https://github.com/DFE-Digital/login.dfe.public-api?tab=readme-ov-file#establishment-types for more information.
/// </remarks>
public enum EstablishmentType
{
    Undeclared = 0,
    
    CommunitySchool = 1,
    VoluntaryAidedSchool = 2,
    VoluntaryControlledSchool = 3,
    
    FoundationSchool = 5,
    CityTechnologyCollege = 6,
    CommunitySpecialSchool = 7,
    NonMaintainedSpecialSchool = 8,
    OtherIndependentSpecialSchool = 10,
    OtherIndependentSchool = 11,
    FoundationSpecialSchool = 12,
    PupilReferralUnit = 14,
    LaNurserySchool = 15,
    
    FurtherEducation = 18,
    
    SecureUnits = 24,
    OffshoreSchools = 25,
    ServiceChildrensEducation = 26,
    Miscellaneous = 27,
    AcademySponsorLed = 28,
    HigherEducationInstitution = 29,
    WelshEstablishment = 30,
    SixthFormCentres = 31,
    SpecialPost16Institution = 32,
    AcademySpecialSponsorLed = 33,
    AcademyConverter = 34,
    FreeSchools = 35,
    FreeSchoolsSpecial = 36,
    BritishOverseasSchools = 37,
    FreeSchoolsAlternativeProvision = 38,
    FreeSchools1619 = 39,
    UniversityTechnicalCollege = 40,
    StudioSchools = 41,
    AcademyAlternativeProvisionConverter = 42,
    AcademyAlternativeProvisionSponsorLed = 43,
    AcademySpecialConverter = 44,
    Academy1619Converter = 45,
    Academy1619SponsorLed = 46,
    ChildrensCentre = 47,
    ChildrensCentreLinkedSite = 48,
    OnlineProvider = 49,
    
    InstitutionFundedByOtherGovernmentDepartment = 56,
    AcademySecure16To19 = 57
}