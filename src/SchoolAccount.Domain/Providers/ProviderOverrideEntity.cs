using SchoolAccount.Kernel;

namespace SchoolAccount.Domain.Providers;

public class ProviderOverrideEntity
{
    public long Id { get; init; }
    
    public string UkPrn { get; init; } = null!;
    public string SchoolName { get; init; } = null!;
    
    public bool HasAccess  { get; init; }
    public SchoolType SchoolType { get; init; }
}