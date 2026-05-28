using System.Globalization;
using Bogus;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Common;
using SchoolAccount.Tests.Common.DataSets;

namespace SchoolAccount.Tests.Common.Builders;

public sealed class OrganisationClaimBuilder(Faker? faker = null)
{
    private readonly Faker _faker = faker ?? new Faker { Random = new Randomizer(1234) };

    private Guid? _id;
    private string? _name;
    private string? _legalName;
    private OrganisationCategory? _category;
    private EstablishmentType? _type;
    private string? _urn;
    private string? _upin;
    private string? _ukprn;
    private OrganisationStateClaim? _status;
    private DateTime? _openedOn;
    private DateTime? _closedOn;
    private string? _address;
    private string? _telephone;
    private IdName<string>? _region;
    private IdCodeName<Guid, string>? _localAuthority;
    private IdName<int>? _phaseOfEducation;
    private int? _statutoryLowAge;
    private int? _statutoryHighAge;
    private string? _districtAdministrativeName;
    private string? _districtAdministrativeCode;

    public static OrganisationClaimBuilder AOrganisationClaim(Faker? faker = null)
    {
        return new(faker);
    }

    public OrganisationClaimBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public OrganisationClaimBuilder WithName(string name)
    {
        _name = name;
        _legalName ??= name;
        return this;
    }

    public OrganisationClaimBuilder WithLegalName(string legalName)
    {
        _legalName = legalName;
        return this;
    }

    public OrganisationClaimBuilder WithCategory(OrganisationCategory category)
    {
        _category = category;
        return this;
    }

    public OrganisationClaimBuilder WithRandomCategory()
    {
        _category = _faker.PickRandom<OrganisationCategory>();
        return this;
    }

    public OrganisationClaimBuilder WithType(EstablishmentType type)
    {
        _type = type;
        return this;
    }

    public OrganisationClaimBuilder WithAddress(string address)
    {
        _address = address;
        return this;
    }

    public OrganisationClaimBuilder WithUrn(string urn)
    {
        _urn = urn;
        return this;
    }

    public OrganisationClaimBuilder WithUpin(string upin)
    {
        _upin = upin;
        return this;
    }

    public OrganisationClaimBuilder WithUkprn(string ukprn)
    {
        _ukprn = ukprn;
        return this;
    }

    public OrganisationClaimBuilder WithPhaseOfEducation(string phase)
    {
        _phaseOfEducation = new IdName<int>
        {
            Name = phase
        };

        return this;
    }

    public OrganisationClaimBuilder WithDefaultPhaseOfEducation(bool assignStatutoryAge = true)
    {
        var type = _faker.PickRandom(SchoolDataSet.SchoolTypes);
        _phaseOfEducation = new IdName<int>
        {
            Id = SchoolDataSet.SchoolTypes.IndexOf(type),
            Name = type
        };

        if (assignStatutoryAge)
        {
            var range = SchoolDataSet.SchoolAgeRanges[type];
            _statutoryLowAge = range.Low;
            _statutoryHighAge = range.High;
        }

        return this;
    }

    public OrganisationClaimBuilder WithLocalAuthority(string name, bool setDistrict = true)
    {
        _localAuthority = new IdCodeName<Guid, string>
        {
            Name = name,
            Id = _faker.Random.Uuid(),
            Code = _faker.Random.Int(0).ToString(CultureInfo.InvariantCulture)
        };

        return setDistrict
            ? WithDistrictAdministrative(name)
            : this;
    }

    public OrganisationClaimBuilder WithDistrictAdministrative(string name, string? code = null)
    {
        _districtAdministrativeName = name.ToShortAuthorityName();
        _districtAdministrativeCode = code ?? LocalAuthorityDataSet.GenerateGssCode(name);
        return this;
    }

    public OrganisationClaimBuilder WithStatutoryLowAge(int age)
    {
        _statutoryLowAge = age;
        return this;
    }

    public OrganisationClaimBuilder WithStatutoryHighAge(int age)
    {
        _statutoryHighAge = age;
        return this;
    }

    public OrganisationClaimBuilder WithRegion(string region)
    {
        _region = new IdName<string>()
        {
            Id = _faker.Random.Uuid().ToString(),
            Name = region
        };

        return this;
    }

    public OrganisationClaimBuilder AsOpen(DateTime? openedOn = null)
    {
        _status = new OrganisationStateClaim()
        {
            Id = 1,
            Name = "Open",
        };
        _openedOn = openedOn;
        return this;
    }

    public OrganisationClaimBuilder AsClosed(DateTime closedOn)
    {
        _status = new OrganisationStateClaim()
        {
            Id = 2,
            Name = "Closed",
        };
        _closedOn = closedOn;
        return this;
    }

    public OrganisationClaimBuilder WithTelephone(string telephone)
    {
        _telephone = telephone;
        return this;
    }

    public OrganisationClaim Build()
    {
        var school = _faker.GetSchoolName(out var lowAge, out var highAge);
        var distinctAuthorityName = string.Empty;
        var authority = _localAuthority ?? BuildLocalAuthority(out distinctAuthorityName);
        var category = BuildCategory();

        return new OrganisationClaim
        {
            Id = _id
                 ?? _faker.Random.Uuid(),
            Name = _name
                   ?? school,
            LegalName = _legalName
                        ?? school.ToUpper(CultureInfo.InvariantCulture),
            Category = category,
            Type = BuildType(category?.Id),
            Urn = _urn,
            Upin = _upin,
            Ukprn = _ukprn
                    ?? _faker.Random.Number(10000000, 19999999).ToString(CultureInfo.InvariantCulture),
            PhaseOfEducation = _phaseOfEducation,
            Address = _address
                      ?? _faker.Address.FullAddress(),
            Region = _region
                     ?? new IdName<string>()
                     {
                         Id = _faker.Random.Uuid().ToString(),
                         Name = _faker.Address.State()
                     },
            LocalAuthority = authority,
            StatutoryLowAge = _statutoryLowAge ?? lowAge,
            StatutoryHighAge = _statutoryHighAge ?? highAge,
            DistrictAdministrativeCode = _districtAdministrativeCode
                                         ?? LocalAuthorityDataSet.GenerateGssCode(distinctAuthorityName),
            DistrictAdministrativeName = _districtAdministrativeName
                                         ?? distinctAuthorityName,
            Status = _status,
            ClosedOn = _closedOn,
            Telephone = _telephone
        };
    }

    public static OrganisationClaimBuilder Academy =>
        AOrganisationClaim()
            .WithName("East Herrington Primary Academy")
            .WithAddress("Balmoral Terrace, East Herrington, Sunderland, Tyne and Wear, SR3 3PR")
            .WithCategory(OrganisationCategory.Establishment)
            .WithType(EstablishmentType.AcademyConverter)
            .WithDefaultPhaseOfEducation()
            .AsOpen();

    public static OrganisationClaimBuilder Trust =>
        AOrganisationClaim()
            .WithName("Abbey Academies Trust")
            .WithAddress("Bourne Abbey C Of E Primary Academy, Abbey Road, Bourne, Not recorded, PE10 9EP")
            .WithCategory(OrganisationCategory.MultiAcademyTrust)
            .AsOpen();

    public static OrganisationClaimBuilder Default => Academy;

    private OrganisationCategoryClaim? BuildCategory()
    {
        return _category.HasValue
            ? new OrganisationCategoryClaim
            {
                Id = _category.Value,
                Name = _category.Value.ToString()
            }
            : null;
    }

    private OrganisationEstablishmentTypeClaim? BuildType(OrganisationCategory? category)
    {
        if (!category.HasValue || category != OrganisationCategory.Establishment)
        {
            return null;
        }

        _type ??= _faker.PickRandom<EstablishmentType>();

        return new OrganisationEstablishmentTypeClaim
        {
            Id = _type.Value,
            Name = _type.Value.ToString()
        };
    }

    private IdCodeName<Guid, string> BuildLocalAuthority(out string authorityName)
    {
        var authority = _faker.GetAuthorityName(out authorityName);
        return new IdCodeName<Guid, string>()
        {
            Name = authority,
            Id = _faker.Random.Uuid(),
            Code = _faker.Random.Number(10000000, 19999999).ToString(CultureInfo.InvariantCulture)
        };
    }
}