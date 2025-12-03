using SchoolAccount.Infrastructure.Teams;

namespace SchoolAccount.Infrastructure.ServiceStatus;

public record ServiceStatusDatabaseEntity(long Id, string Name, string Description)
{
    public required TeamDatabaseEntity ServiceStatus { get; set; }
};
