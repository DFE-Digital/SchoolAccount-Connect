namespace SchoolAccount.Domain.Entities;

public partial class RequirementEntity
{
    public int Id { get; init; }

    public required string Name { get; set; }

    public string? Description { get; set; }
}
