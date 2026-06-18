namespace PlaywrightTests.DfE.UIMapping.Forms;

//This class contains all the values that SubTaskFormData will hold.
//Add any support functions to SubTaskFormDataHelpers.cs
public partial class SubTaskFormData : FormDataBase
{
    public int TaskId { get; set; } = 0;
    public int ServiceId { get; set; } = 0;
    public string SubTaskReferenceNo { get; set; } = string.Empty;
    public string SubTaskName { get; set; } = string.Empty;
    public string SubTaskDescription { get; set; } = string.Empty;
    public string DigitalTaskLink { get; set; } = string.Empty;
    public int RequirementId { get; set; } = 0;
    public DateTime StartDate { get; set; } = DateTime.Today;
    public bool StartDateIsExact { get; set; } = false;
    public DateTime DueDate { get; set; } = DateTime.Today;
    public bool DueDateIsExact { get; set; } = false;
    public DateTime ExpiryDate { get; set; } = DateTime.Today;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? DateCreated { get; set; } = DateTime.UtcNow;
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime? DateUpdated { get; set; } = DateTime.UtcNow;
    public int WorkflowStateId { get; set; } = 1;
    public string Comment { get; set; } = string.Empty;
    public int Version { get; set; } = 0;
    public bool IsDeleted { get; set; } = false;
    public string DisplayDate { get; set; } = String.Empty;
    public string ArchiveComment { get; set; } = string.Empty;
    public int TeamId { get; set; } = 0;
}