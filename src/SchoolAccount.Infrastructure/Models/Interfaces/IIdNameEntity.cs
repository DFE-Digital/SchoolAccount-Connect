namespace SchoolAccount.Infrastructure.Models.Interfaces;

public interface IIdNameEntity<T> : IDatabaseEntity
    where T : struct
{
    public T Id { get; set; }
    public string Name { get; set; }
}
