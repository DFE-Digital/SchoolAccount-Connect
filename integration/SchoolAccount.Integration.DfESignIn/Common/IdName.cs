namespace SchoolAccount.Integration.DfESignIn.Common;

public class IdName<TId>
{
    public TId? Id { get; init; }
    public string? Name { get; init; }
    
    public override string ToString() => $"({Id}) {Name}";
}