using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Kernel.Conditions.Interface;
using SchoolAccount.Kernel.Organisations;

namespace SchoolAccount.Kernel.Conditions;

public class SmartDataProbabilityOfDeclining : IConditionMapper
{
    public string Identifier => "SmartData.ProbabilityOfDeclining";
    
    public Task<object?> Resolve(object data, CancellationToken cancellationToken)
    {
        object? value = data switch
        {
            AcademyEstablishment establishment => establishment.SmartData?.ProbabilityOfDeclining,
            AcademyOrganisation organisation => organisation.SmartData?.ProbabilityOfDeclining,
            _ => null
        };

        return Task.FromResult(value);
    }
}