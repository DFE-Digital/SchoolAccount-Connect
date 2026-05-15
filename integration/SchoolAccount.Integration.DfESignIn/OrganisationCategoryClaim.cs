using System.Text.Json.Serialization;
using SchoolAccount.Integration.DfESignIn.Common;
using SchoolAccount.Integration.DfESignIn.Converters;

namespace SchoolAccount.Integration.DfESignIn;

public record OrganisationCategoryClaim : IdName<OrganisationCategory>
{
    [JsonConverter(typeof(OrganisationCategoryConverter))]
    public new OrganisationCategory Id { get; init; }
}
