using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SchoolAccount.Integration.AcademiesApi.Models;

[SuppressMessage("Usage", "CA2227:Collection properties should be read only")]
[SuppressMessage("Design", "CA1002:Do not expose generic lists")]
public class AcademyTrust
{
    [JsonPropertyName("ifdData")]
    public TrustIfdData? IfdData { get; set; }

    [JsonPropertyName("giasData")]
    public TrustGiasData? GiasData { get; set; }

    public List<AcademyEstablishment> Establishments { get; set; } = [];
}