namespace SchoolAccount.Domain.Interfaces;

public interface IIdNameEntity<T> : IDatabaseEntity
    where T : struct
{
    T Id { get; init; }
    string Name { get; set; }
}
