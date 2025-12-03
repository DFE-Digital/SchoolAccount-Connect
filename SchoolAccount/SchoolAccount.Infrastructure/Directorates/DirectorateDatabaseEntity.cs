using SchoolAccount.Infrastructure.Teams;

namespace SchoolAccount.Infrastructure.Directorates;

public class DirectorateDatabaseEntity
{
    public long Id { get; init; }

    public string Name { get; init; }

    public string? Description { get; init; }

    public long? GroupId { get; init; } // TODO This should not be nullable in the database

    public ICollection<TeamDatabaseEntity> Teams { get; init; }
}
