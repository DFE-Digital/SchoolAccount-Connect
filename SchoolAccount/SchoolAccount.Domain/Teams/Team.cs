namespace SchoolAccount.Domain.Teams;

public class Team
{
    public long Id { get; init; }
    public string Name { get; init; }
    public string? Acronym { get; init; }
    public string? Description { get; init; }
}
