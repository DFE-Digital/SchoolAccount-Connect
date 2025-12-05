using SchoolAccount.Infrastructure.Teams;

namespace SchoolAccount.Infrastructure.Directorates;

public class DirectorateDatabaseEntity
{
    public long Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; } = string.Empty;

    public long? GroupId { get; init; } // TODO This should not be nullable in the database

    public required ICollection<TeamDatabaseEntity> Teams { get; init; }
}
