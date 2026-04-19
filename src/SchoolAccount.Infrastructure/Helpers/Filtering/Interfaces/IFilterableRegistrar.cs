namespace SchoolAccount.Infrastructure.Helpers.Filtering.Interfaces;

public interface IFilterableRegistrar
{
    Type TypeBeingRegistered { get; }
    FieldSelector FieldSelectorsBeingRegistered { get; }
}

public interface IFilterableRegistrar<TFilter> : IFilterableRegistrar
{
    void ConsolidateFilters(TFilter filter);
}
