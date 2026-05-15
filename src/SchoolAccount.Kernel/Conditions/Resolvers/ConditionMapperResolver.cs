using System.Collections.ObjectModel;
using SchoolAccount.Kernel.Conditions.Interface;

namespace SchoolAccount.Kernel.Conditions.Resolvers;

public class ConditionMapperResolver(IEnumerable<IConditionMapper> conditionMappers) : IConditionMapperResolver
{
    public async Task<Collection<OrganisationCondition>> Resolve(object organisation)
    {
        var conditions = new Collection<OrganisationCondition>();

        foreach (var mapper in conditionMappers)
        {
            var outcome = await mapper.Resolve(organisation, CancellationToken.None);

            if (outcome is not null)
            {
                conditions.Add(new OrganisationCondition(mapper.Identifier, outcome));
            }
        }

        return conditions;
    }
}