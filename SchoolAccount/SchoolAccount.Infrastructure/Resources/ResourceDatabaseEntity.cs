namespace SchoolAccount.Infrastructure.Resources;

public class ResourceDatabaseEntity
{
    public long Id { get; set; }
    
    public string ServiceName { get; set; }
    
    public string ServiceDescription { get; set; }
    
    public string DigitalLink { get; set; }
}