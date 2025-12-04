using SchoolAccount.Infrastructure.Directorates;
using SchoolAccount.Infrastructure.Models;
using SchoolAccount.Infrastructure.ServiceStatus;

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

namespace SchoolAccount.Infrastructure.Teams;

public class TeamDatabaseEntity : AuditableDatabaseEntity
{
    public long Id { get; init; }

    public required string ServiceName { get; init; }

    public string? Acronym { get; init; }

    public string? ServiceDescription { get; init; }

    public string? DigitalServiceLink { get; init; }

    public long? DirectorateId { get; init; }

    public required long ServiceStatusId { get; init; }

    public DirectorateDatabaseEntity? Directorate { get; init; }

    public ServiceStatusDatabaseEntity? ServiceStatus { get; init; }
}
