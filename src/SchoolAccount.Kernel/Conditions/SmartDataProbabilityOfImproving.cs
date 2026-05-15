using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Kernel.Conditions.Interface;
using SchoolAccount.Kernel.Organisations;

namespace SchoolAccount.Kernel.Conditions;

public class SmartDataProbabilityOfImproving : IConditionMapper
{
    public string Identifier => "SmartData.ProbabilityOfImproving";
    
    public Task<object?> Resolve(object data, CancellationToken cancellationToken)
    {
        object? value = data switch
        {
            AcademyEstablishment establishment => establishment.SmartData?.ProbabilityOfImproving,
            AcademyOrganisation organisation => organisation.SmartData?.ProbabilityOfImproving,
            _ => null
        };

        return Task.FromResult(value);
    }
}