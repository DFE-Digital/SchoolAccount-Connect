namespace SchoolAccount.Kernel;

public abstract class Entity(long id)
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected Entity()
        : this(0) { }

    public long Id { get; } = id;
    public string CreatedBy { get; init; } = string.Empty;
    public DateTime DateCreated { get; set; }
    public string UpdatedBy { get; init; } = string.Empty;
    public DateTime DateUpdated { get; set; }

    public IEnumerable<IDomainEvent> DomainEvents => [.. _domainEvents];

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
