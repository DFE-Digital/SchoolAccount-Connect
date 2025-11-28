namespace SchoolAccount.Infrastructure;

internal interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}