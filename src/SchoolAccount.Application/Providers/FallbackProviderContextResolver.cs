using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Providers;

public class FallbackProviderContextResolver(IFallbackProviderResolver fallbackProviderResolver) : IProviderContextResolver
{
    public Type ProviderType { get; } = typeof(FallbackProvider);

    public OrganisationClaim? Resolve(OrganisationClaim? organisationClaim)
    {
        if (organisationClaim is null)
        {
            return null;
        }

        if (!fallbackProviderResolver.TryGetProvider(organisationClaim.Ukprn, out var fallbackProvider))
        {
            return organisationClaim;
        }

        OrganisationCategory categoryId;
        EstablishmentType? establishmentType;

        switch (fallbackProvider.SchoolType)
        {
            case SchoolType.Academy:
            case SchoolType.AcademySpecial:
                categoryId = OrganisationCategory.Establishment;
                establishmentType = EstablishmentType.AcademyConverter;
                break;

            case SchoolType.SingleAcademyTrust:
                categoryId = OrganisationCategory.SingleAcademyTrust;
                establishmentType = null;
                break;

            case SchoolType.MultiAcademyTrust:
                categoryId = OrganisationCategory.MultiAcademyTrust;
                establishmentType = null;
                break;

            default:
                throw new NotImplementedException(fallbackProvider.SchoolType.ToString());
        }

        return organisationClaim with
        {
            Category = new OrganisationCategoryClaim()
            {
                Id = categoryId,
                Name = categoryId.ToString()
            },
            Type = establishmentType.HasValue
                ? new OrganisationEstablishmentTypeClaim()
                {
                    Id = establishmentType.Value,
                    Name = establishmentType.Value.ToString()
                }
                : null
        };
    }
}