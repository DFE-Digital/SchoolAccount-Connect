namespace SchoolAccount.Integration.DfESignIn.Common;

public record IdName<TId>
{
    public TId? Id { get; init; }
    public string? Name { get; init; }

    public override string ToString() => $"({Id}) {Name}";
}
