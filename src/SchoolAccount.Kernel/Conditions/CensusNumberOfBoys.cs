using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Kernel.Conditions.Interface;
using SchoolAccount.Kernel.Organisations;

namespace SchoolAccount.Kernel.Conditions;

public class CensusNumberOfBoys : IConditionMapper
{
    public string Identifier => "Census.NumberOfBoys";
    
    public Task<object?> Resolve(object data, CancellationToken cancellationToken)
    {
        object? value = data switch
        {
            AcademyEstablishment establishment => establishment.Census?.NumberOfBoys,
            AcademyOrganisation organisation => organisation.Census?.NumberOfBoys,
            _ => null
        };

        return Task.FromResult(value);
    }
}