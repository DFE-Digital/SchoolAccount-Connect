using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Kernel.Conditions.Interface;
using SchoolAccount.Kernel.Organisations;

namespace SchoolAccount.Kernel.Conditions;

public class CensusNumberOfPupils : IConditionMapper
{
    public string Identifier => "Census.NumberOfPupils";
    
    public Task<object?> Resolve(object data, CancellationToken cancellationToken)
    {
        object? value = data switch
        {
            AcademyEstablishment establishment => establishment.Census?.NumberOfPupils,
            AcademyOrganisation organisation => organisation.Census?.NumberOfPupils,
            _ => null
        };

        return Task.FromResult(value);
    }
}