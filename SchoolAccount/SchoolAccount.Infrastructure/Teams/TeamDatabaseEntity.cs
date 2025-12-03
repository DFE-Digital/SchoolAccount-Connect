using SchoolAccount.Infrastructure.Directorates;
using SchoolAccount.Infrastructure.ServiceStatus;

namespace SchoolAccount.Infrastructure.Teams;

public class TeamDatabaseEntity
{
    public long Id { get; set; }
    
    public string ServiceName { get; set; }
    
    public string? Acronym { get; set; }
    
    public string ServiceDescription { get; set; }
    
    public string DigitalServiceLink { get; set; }
    
    public long? DirectorateId { get; set; }

    //todo set required?
    public long ServiceStatusId { get; set; }
    
    public DirectorateDatabaseEntity? Directorate { get; set; }
    
    public ServiceStatusDatabaseEntity ServiceStatus { get; set; }
}