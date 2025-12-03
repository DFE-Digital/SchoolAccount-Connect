namespace SchoolAccount.Domain;

public record ServiceStatus
{
    private ServiceStatus(long id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public long Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public static readonly ServiceStatus Active = new(
        2,
        "Active",
        "The service is active and operational."
    );
    public static readonly ServiceStatus Draft = new(
        1,
        "Draft",
        "The service is inactive and not in use."
    );
    public static readonly ServiceStatus DueToBeDecommissioned = new(
        3,
        "DueToBeDecommissioned",
        "The service is inactive and not in use."
    );
    public static readonly ServiceStatus Decommissioned = new(4, "Decommissioned", null);
}
