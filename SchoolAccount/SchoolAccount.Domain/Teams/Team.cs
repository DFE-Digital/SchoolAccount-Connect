using SchoolAccount.Kernel;

namespace SchoolAccount.Domain.Teams;

public class Team : Entity
{
    private Team(string name, string emailAddress, string description, string? acronym)
    {
        Name = name;
        Acronym = acronym;
        Description = description;
        EmailAddress = emailAddress;
        ServiceStatus = ServiceStatus.Draft;
    }

    public string Name { get; private set; }
    public string? Acronym { get; private set; }
    public string Description { get; private set; }
    public string EmailAddress { get; private set; }
    public ServiceStatus ServiceStatus { get; private set; }

    public static Team Create(string name, string emailAddress, string description, string? acronym)
    {
        return new Team(name, emailAddress, description, acronym);
    }
}
