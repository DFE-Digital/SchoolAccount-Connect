namespace SchoolAccount.Domain.Teams;

public class Team
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Acronym { get; init; }
    public string? Description { get; init; }
}
