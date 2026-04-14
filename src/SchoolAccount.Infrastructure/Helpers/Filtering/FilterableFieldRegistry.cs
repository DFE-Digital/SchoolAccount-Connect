using System.Collections.ObjectModel;
using System.Linq.Expressions;
using SchoolAccount.Infrastructure.Helpers.Filtering.Interfaces;

namespace SchoolAccount.Infrastructure.Helpers.Filtering;

public class FilterableFieldRegistry
{
    private readonly FieldSelectorMapping _all = new();
    private readonly List<IFilterableRegistrar> _registrars;

    public FilterableFieldRegistry(IEnumerable<IFilterableRegistrar> registrars)
    {
        _registrars = registrars.ToList();
        
        foreach (var registrar in _registrars)
        {
            _all.TryAdd(registrar.TypeBeingRegistered, registrar.FieldSelectorsBeingRegistered);
        }
    }

    public FieldSelector GetSelectorsForType(Type type)
    {
        return _all.TryGetValue(type, out var selectors)
            ? selectors
            : throw new InvalidDataException($"No selectors have been registered for type \"{type.Name}\".");
    }

    public LambdaExpression GetSelectorForType(Type type, string fieldName)
    {
        return GetSelectorsForType(type).TryGetValue(fieldName, out var selector)
            ? selector
            : throw new InvalidDataException(
                $"No selector with name \"{fieldName}\" has been registered for type \"{type.Name}\".");
    }

    public FieldSelectorMapping All => _all;
    public IReadOnlyCollection<IFilterableRegistrar> Registrars => new Collection<IFilterableRegistrar>(_registrars);
}