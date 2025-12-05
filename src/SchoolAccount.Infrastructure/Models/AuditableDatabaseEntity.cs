namespace SchoolAccount.Infrastructure.Models;

public abstract class AuditableDatabaseEntity : IDatabaseEntity
{
    public long Id { get; init; }

    public string CreatedBy { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public string UpdatedBy { get; set; } = null!;

    public DateTime DateUpdated { get; set; }
}
