using PlaywrightTests.GlobalConstants.Global;

namespace PlaywrightTests.DfE.UIMapping.Forms;

/// <summary>
/// Represents the data filled into the form on the resource journey pages.
/// </summary>
/// <remarks>We can use this class to generate an entire set of Resource Data.
/// Note there is a silent dependency here that the names of the properties exactly match those of the database table.
/// This is because, in order to keep things simple, we are currently not using any form of mapper.
/// </remarks>
public partial class ResourceFormData : FormDataBase
{
    public string ResourceName { get; set; } = string.Empty;
    public string ResourceDescription { get; set; } = string.Empty;
    public string DigitalLink { get; set; } = string.Empty;
    public int TeamId { get; set; } = 0;
    public string TeamName { get; set; } = string.Empty;
    public int ResourceTypeId { get; set; } = 0;
    public int? GuidanceTypeId { get; set; } = null;
    public int ResourceStatusId { get; set; } = 2; // Always 2 for some reason
    public string GovUkLastUpdated { get; set; } = string.Empty; // YYYY-MM-DD format
    public string CreatedBy { get; set; } = Global.TestEmailAddress;
    public string UpdatedBy { get; set; } = Global.TestEmailAddress;
    public string DateCreated { get; set; } = string.Empty; // 2025-03-26 10:33:35.033 format
    public string DateUpdated { get; set; } = string.Empty; // 2025-03-26 10:33:35.033 format
    public bool? IsDeleted { get; set; } = null; //Doesn't seem to be used yet
    public int? Version { get; set; } = null; // Doesn't seem to be used yet either
    public int WorkflowStateId { get; set; } = 0;
}