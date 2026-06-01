using System.Text.Json.Serialization;
using SchoolAccount.Integration.DfESignIn.Common;

namespace SchoolAccount.Integration.DfESignIn.Models;

public record OrganisationStateClaim : IdName<int>
{
    [JsonPropertyName("tagColor")]
    public string? TagColour { get; set; }
}
