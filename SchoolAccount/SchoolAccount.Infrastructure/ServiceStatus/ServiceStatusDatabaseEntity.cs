using SchoolAccount.Infrastructure.Teams;

namespace SchoolAccount.Infrastructure.ServiceStatus;

public record ServiceStatusDatabaseEntity(long Id, string Name, string Description)
{
    public TeamDatabaseEntity ServiceStatus { get; set; }
};