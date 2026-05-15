using System.Collections.ObjectModel;

namespace SchoolAccount.Kernel.Conditions.Interface;

public interface IConditionMapperResolver
{
    Task<Collection<OrganisationCondition>> Resolve(object organisation);
}