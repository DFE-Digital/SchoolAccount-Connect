using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Kernel.Conditions.Interface;
using SchoolAccount.Kernel.Organisations;

namespace SchoolAccount.Kernel.Conditions;

public class CensusNumberOfGirls : IConditionMapper
{
    public string Identifier => "Census.NumberOfGirls";
    
    public Task<object?> Resolve(object data, CancellationToken cancellationToken)
    {
        object? value = data switch
        {
            AcademyEstablishment establishment => establishment.Census?.NumberOfGirls,
            AcademyOrganisation organisation => organisation.Census?.NumberOfGirls,
            _ => null
        };

        return Task.FromResult(value);
    }
}