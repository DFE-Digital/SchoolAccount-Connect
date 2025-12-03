namespace SchoolAccount.Infrastructure.Resources;

public class ResourceDatabaseEntity
{
    public long Id { get; init; }

    public string ServiceName { get; init; } = string.Empty;

    public string ServiceDescription { get; init; } = string.Empty;

    public string DigitalLink { get; init; } = string.Empty;
}
