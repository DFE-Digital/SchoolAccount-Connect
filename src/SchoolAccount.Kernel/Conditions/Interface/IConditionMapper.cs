namespace SchoolAccount.Kernel.Conditions.Interface;

public interface IConditionMapper
{
    public string Identifier { get; }
    public Task<object?> Resolve(object data, CancellationToken cancellationToken);
}