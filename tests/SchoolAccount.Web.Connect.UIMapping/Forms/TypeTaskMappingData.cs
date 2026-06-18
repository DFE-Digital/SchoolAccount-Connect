namespace PlaywrightTests.DfE.UIMapping.Forms;

//There aren't any helpers for this because the fields all need to be manually set
public class TypeTaskMappingData : FormDataBase
{
    public long TaskId { get; set; } = 0;
    public int TypeId { get; set; } = 0;
}