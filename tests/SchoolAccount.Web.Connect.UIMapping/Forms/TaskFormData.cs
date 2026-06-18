namespace PlaywrightTests.DfE.UIMapping.Forms;

//This class contains all the values that TaskFormData will hold.
//Add any support functions to TaskFormDataHelpers.cs
public partial class TaskFormData : FormDataBase
{
    public string TaskReferenceNo { get; set; } = string.Empty;
    public string TaskName { get; set; } = string.Empty;
    public string TaskDescription { get; set; } = string.Empty;
    public int? ServiceId { get; set; } = null;
    public DateTime PublishDate { get; set; } = DateTime.Today;
    public int RequirementId { get; set; } = 3;
    public int WorkflowStateId { get; set; } = 1;
    public bool IsDeleted { get; set; } = false;
    public string PublishComment { get; set; } = string.Empty;
    public string ArchiveComment { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? DateCreated { get; set; } = DateTime.UtcNow;
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime? DateUpdated { get; set; } = DateTime.UtcNow;
    public int TeamId { get; set; } = 0;
    public int Version { get; set; } = 0;
    public bool IsLatestVersion { get; set; } = true;
}