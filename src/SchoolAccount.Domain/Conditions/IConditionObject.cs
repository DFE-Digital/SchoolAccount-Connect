namespace SchoolAccount.Domain.Conditions;

public interface IConditionObject
{
    string Identifier { get; }
    ConditionComparitorType ComparitorType { get; }
    object? Value { get; }
}