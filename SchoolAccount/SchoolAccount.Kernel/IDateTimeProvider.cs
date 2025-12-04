namespace SchoolAccount.Kernel;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
