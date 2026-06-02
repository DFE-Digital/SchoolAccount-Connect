using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Application.Features.Shared.Filtering.Models;

namespace SchoolAccount.Application.Abstractions;

public interface IFilter : IList<FilterRequest>
{
    
}