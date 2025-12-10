using System.ComponentModel.DataAnnotations;

namespace SchoolAccount.Application.Features.Shared;

public abstract class BaseListItem
{
    public long Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string TeamName { get; set; } = null!;
    
    public string UpdatedBy { get; set; } = null!;
    
    public DateTime DateUpdated { get; set; }
    
    public string WorkflowStateName { get; set; } = null!;
};