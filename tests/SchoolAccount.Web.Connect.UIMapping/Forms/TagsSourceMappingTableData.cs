namespace PlaywrightTests.DfE.UIMapping.Forms;

//There aren't any helpers for this because the fields all need to be manually set
public partial class TagsSourceMappingTableData : FormDataBase
{
// Entity Id is the Id of the Task or Subtask being referenced.
    public long EntityId { get; set; } = 0;

// This is 3 for a Subtask, 2 for a Task, 1 for Service, 4 for Resource
    public int SourceId { get; set; } = 0;
    public long TagId { get; set; } = 0;
}