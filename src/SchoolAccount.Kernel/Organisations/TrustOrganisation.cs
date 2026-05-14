using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Kernel.Organisations;

public class TrustOrganisation(string ukrpn, string name) : IOrganisation
{
    public TrustOrganisation(OrganisationClaim claim)
        : this(claim.UkPrn!, claim.Name!) { }

    public TrustOrganisation(AcademyTrust trust)
        : this(trust.GiasData!.Ukprn!, trust.GiasData!.GroupName!)
    {
        Establishments = trust.Establishments
            .Select(x => new EstablishmentOrganisation(new Organisation(x)))
            .ToList();
    }

    public TrustOrganisation(Organisation organisation)
    : this(organisation.UkPrn, organisation.Name)
    {
        Establishments = organisation.Children?.Select(x => new EstablishmentOrganisation(x)).ToList() ?? [];
    }

    public string Ukrpn { get; } = ukrpn;
    public string Name { get; } = name;
    public IReadOnlyCollection<EstablishmentOrganisation> Establishments { get; } = [];
}
