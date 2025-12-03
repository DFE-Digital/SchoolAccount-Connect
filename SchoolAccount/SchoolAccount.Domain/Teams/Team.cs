using SchoolAccount.Kernel;

namespace SchoolAccount.Domain.Teams;

public class Team : Entity
{
    public string Name { get; set; } = string.Empty;
    public string? Acronym { get; set; }
    public string Description { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public ServiceStatus ServiceStatus { get; set; }
    
    public static Team Create(string name, string emailAddress, string description, string? acronym)
    {
        return new Team
        {
            Name = name,
            Acronym = acronym,
            Description = description,
            EmailAddress = emailAddress,
            ServiceStatus = ServiceStatus.Draft
        };
    }
}