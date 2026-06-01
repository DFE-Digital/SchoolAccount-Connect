using System.Globalization;
using Bogus;
using Microsoft.FeatureManagement;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Models;
using SchoolAccount.Integration.DfESignIn.Providers;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Organisations;
using SchoolAccount.Tests.Common.Context;
using SchoolAccount.Tests.Common.DataSets;
using SchoolAccount.Tests.Common.Fakes;

namespace SchoolAccount.Tests.Common.Builders;

public sealed class OrganisationContextBuilder(Faker? faker = null, IFeatureManager? featureManager = null)
{
    private readonly Faker _faker = faker ?? new Faker { Random = new Randomizer(1234) };
    private readonly IFeatureManager _featureManager = featureManager ?? new FakeFeatureManager();

    private IProvider? _provider;
    private IOrganisation? _organisation;
    private SchoolType? _schoolType;

    public static OrganisationContextBuilder AOrganisationContext(Faker? faker = null, IFeatureManager? featureManager = null)
    {
        return new(faker, featureManager);
    }

    public OrganisationContextBuilder WithProvider(IProvider provider)
    {
        _provider = provider;
        return this;
    }

    public OrganisationContextBuilder WithOrganisation(IOrganisation organisation)
    {
        _organisation = organisation;
        return this;
    }

    public OrganisationContextBuilder WithSchoolType(SchoolType schoolType)
    {
        _schoolType = schoolType;

        if (_provider is null)
        {
            _provider = schoolType switch
            {
                SchoolType.Academy or SchoolType.AcademySpecial => new PreSixteenProvider(),
                SchoolType.LocalAuthorityManaged or SchoolType.LocalAuthorityManagedSpecial => new LamsProvider(_featureManager),
                SchoolType.SingleAcademyTrust or SchoolType.MultiAcademyTrust => new TrustProvider(),
                SchoolType.NonMaintainedSpecial or SchoolType.IndustrySpecial => new SpecialsProvider(_featureManager),
                _ => null   
            };
        }

        if (_organisation is null)
        {
            var ukPrn = _faker.Random.Number(10000000, 19999999).ToString(CultureInfo.InvariantCulture);
            var name = _faker.GetSchoolName();

            _organisation = schoolType switch
            {
                SchoolType.Academy 
                or SchoolType.AcademySpecial 
                    => new EstablishmentOrganisation(ukPrn, name),
                SchoolType.LocalAuthorityManaged 
                or SchoolType.LocalAuthorityManagedSpecial 
                    => new LocalAuthorityOrganisation(ukPrn, name),
                SchoolType.SingleAcademyTrust 
                or SchoolType.MultiAcademyTrust 
                    => new TrustOrganisation
                    {
                        Ukrpn = ukPrn,
                        Name = name,
                        Establishment = EstablishmentType.Undeclared,
                        Category = OrganisationCategory.Undeclared
                    },
                _ => null
            };
        }

        return this;
    }

    public TestOrganisationContext Build()
    {
        return new TestOrganisationContext(
            _provider, 
            _organisation, 
            _schoolType
        );
    }

    public static implicit operator TestOrganisationContext(OrganisationContextBuilder builder)
    {
        return builder.Build();
    }
}