namespace SchoolAccount.Web.Connect.Settings;

public record AzureAppConfigurationSettings
{
    public const string SectionName = "AzureAppConfiguration";

    public bool Enabled { get; init; }

    public bool IsEmulated { get; init; }

    public string Endpoint { get; init; } = string.Empty;
}
