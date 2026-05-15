using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Kernel.Conditions.Interface;
using SchoolAccount.Kernel.Organisations;

namespace SchoolAccount.Kernel.Conditions;

public class SmartDataRiskRatingNum : IConditionMapper
{
    public string Identifier => "SmartData.RiskRatingNum";
    
    public Task<object?> Resolve(object data, CancellationToken cancellationToken)
    {
        object? value = data switch
        {
            AcademyEstablishment establishment => establishment.SmartData?.RiskRatingNum,
            AcademyOrganisation organisation => organisation.SmartData?.RiskRatingNum,
            _ => null
        };

        return Task.FromResult(value);
    }
}