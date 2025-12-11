namespace SchoolAccount.Kernel;

public interface IUserContext
{
    string? AuthenticationType { get; }
    bool IsAuthenticated { get; }
    string? Id { get; }
    string? EmailAddress { get; }
    string? Name { get; }
    string? PreferredName { get; }
}
