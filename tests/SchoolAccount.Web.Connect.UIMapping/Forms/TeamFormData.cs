namespace PlaywrightTests.DfE.UIMapping.Forms;

//Add any functions for manipulating this data in TeamFormDataHelpers.cs
public partial class TeamFormData : FormDataBase
{
    // Database-mapped properties (exact column name matches)
    public string TeamName { get; set; } = string.Empty;
    public string Acronym { get; set; } = string.Empty;
    public string TeamDescription { get; set; } = string.Empty;
    public int GroupId { get; set; } = 0;
    public int DirectorateId { get; set; } = 0;
    public string DeputyDirector { get; set; } = string.Empty;
    public string TeamInboxEmail { get; set; } = string.Empty;
    public string TeamOwnerNames { get; set; } = string.Empty;
    public int WorkflowStateId { get;set; } = 0;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public string DigitalServiceLink { get; set; } = string.Empty;
    public int TeamStatusId { get; set; } = 0;
    public DateTime? DueToDecommissionDate { get; set; }
    public bool IsExactDecommissionDate { get; set; } = false;
    public int SupportLevelId { get; set; } = 0;
}