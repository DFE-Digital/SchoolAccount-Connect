using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Kernel.Conditions.Interface;
using SchoolAccount.Kernel.Organisations;

namespace SchoolAccount.Kernel.Conditions;

public class SmartDataProbabilityOfStayingTheSame : IConditionMapper
{
    public string Identifier => "SmartData.ProbabilityOfStayingTheSame";
    
    public Task<object?> Resolve(object data, CancellationToken cancellationToken)
    {
        object? value = data switch
        {
            AcademyEstablishment establishment => establishment.SmartData?.ProbabilityOfStayingTheSame,
            AcademyOrganisation organisation => organisation.SmartData?.ProbabilityOfStayingTheSame,
            _ => null
        };

        return Task.FromResult(value);
    }
}