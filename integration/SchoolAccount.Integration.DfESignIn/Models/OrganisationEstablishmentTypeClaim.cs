using System.Text.Json.Serialization;
using SchoolAccount.Integration.DfESignIn.Common;
using SchoolAccount.Integration.DfESignIn.Converters;

namespace SchoolAccount.Integration.DfESignIn.Models;

public record OrganisationEstablishmentTypeClaim : IdName<EstablishmentType>
{
    [JsonConverter(typeof(EstablishmentTypeConverter))]
    public new EstablishmentType Id { get; init; }
}
