namespace SchoolAccount.Domain.Teams;

public class Team
{
    public long Id { get; init; }
    public string Name { get; init; }
    public string? Acronym { get; init; }
    public string? Description { get; init; }

    public long? ResourceId { get; init; }
    public string? ResourceTypeName { get; init; }
    public string? ResourceLink { get; init; }

    public long? GroupId { get; init; }
    public string? GroupName { get; init; }

    public long? DirectorateId { get; set; }
    public string? DirectorateName { get; set; }


}