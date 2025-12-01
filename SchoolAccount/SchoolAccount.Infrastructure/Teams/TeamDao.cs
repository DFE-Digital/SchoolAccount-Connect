namespace SchoolAccount.Infrastructure.Teams;

public class TeamDao
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public long? ResourceId { get; set; }
    public ResourceDao? Resource { get; set; }
}