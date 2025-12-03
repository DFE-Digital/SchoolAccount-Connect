using SchoolAccount.Infrastructure.Directorates;
using SchoolAccount.Infrastructure.ServiceStatus;

namespace SchoolAccount.Infrastructure.Teams;

public class TeamDatabaseEntity
{
    // Properties
    public long Id { get; set; }

    public required string ServiceName { get; set; }

    public string? Acronym { get; set; }

    public string? ServiceDescription { get; set; }

    public string? DigitalServiceLink { get; set; }

    public long? DirectorateId { get; set; }

    public required long ServiceStatusId { get; set; }

    // Navigation Properties
    public DirectorateDatabaseEntity? Directorate { get; set; }

    public ServiceStatusDatabaseEntity? ServiceStatus { get; set; }
}
