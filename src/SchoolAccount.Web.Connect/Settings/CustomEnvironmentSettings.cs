using System.Text.Json.Serialization;

namespace SchoolAccount.Web.Connect.Settings;

public class CustomEnvironmentSettings
{
    public const string SectionName = "Environment";

    public string? Label { get; set; }

    [JsonIgnore]
    public bool HasLabel => !string.IsNullOrWhiteSpace(Label);
}
