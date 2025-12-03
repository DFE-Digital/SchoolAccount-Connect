using SchoolAccount.Infrastructure.Directorates;

namespace SchoolAccount.Infrastructure.Teams;

public class TeamDatabaseEntity
{
    public long Id { get; init; }

    public string ServiceName { get; init; } = string.Empty;

    public string Acronym { get; init; } = string.Empty;

    public string ServiceDescription { get; init; } = string.Empty;

    public string DigitalServiceLink { get; init; } = string.Empty;

    public long? DirectorateId { get; init; }

    public DirectorateDatabaseEntity? Directorate { get; init; }
}
