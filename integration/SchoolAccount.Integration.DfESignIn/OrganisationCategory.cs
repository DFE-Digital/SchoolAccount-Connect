namespace SchoolAccount.Integration.DfESignIn;

/// <summary>
/// All the supported organisational types from DSI
/// </summary>
/// <remarks>
/// Go to https://github.com/DFE-Digital/login.dfe.public-api?tab=readme-ov-file#organisation-categories for more information.
/// </remarks>
public enum OrganisationCategory
{
    Undeclared = 0,
    
    /// <remarks>
    /// If picked, go to <see cref="EstablishmentType"/> for further information.
    /// </remarks>
    Establishment = 1,
    LocalAuthority = 2,
    OtherLegacyOrganisations = 3,
    EarlyYearSetting = 4,
    
    OtherStakeholders = 8,
    TrainingProviders = 9,
    MultiAcademyTrust = 10,
    Government = 11,
    OtherGiasStakeholder = 12,
    SingleAcademyTrust = 13,
    
    SoftwareSuppliers = 50,
    FurtherEducation = 51,
}