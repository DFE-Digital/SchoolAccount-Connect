using Bogus;
using PlaywrightTests.GlobalConstants.Global;

namespace PlaywrightTests.DfE.UIMapping.Forms;

//This class contains all the values that SubTaskFormData will hold.
//Add any support functions to SubTaskFormDataHelpers.cs
public partial class SubTaskFormData : FormDataBase
{
    private SubTaskFormData() { }
    public static SubTaskFormData GenerateRandomData()
    {
        var stfData = new SubTaskFormData();

        stfData.ServiceId = _faker.Random.Int(1, 1000); //This should also have a DB lookup
        stfData.SubTaskReferenceNo = _faker.Random.AlphaNumeric(10);
        stfData.SubTaskName = $"{_faker.Commerce.ProductName()}-{Guid.NewGuid().ToString()[..8]}";
        stfData.SubTaskDescription = _faker.Lorem.Paragraph();
        stfData.DigitalTaskLink = _faker.Internet.Url();
        var requirements = FormHelpers.RequirementIdMapping.Keys.ToArray();
        var selectedRequirement = _faker.PickRandom(requirements);
        stfData.RequirementId = FormHelpers.GetRequirementId(selectedRequirement);
        stfData.StartDate = _faker.Date.Recent(); //Recent = Sometime in the last month
        stfData.StartDateIsExact = _faker.Random.Bool(); //Unsure if this should be random or always false unless specified
        stfData.DueDate = _faker.Date.Soon(1); //Future = Next year
        stfData.DueDateIsExact = _faker.Random.Bool(); //Again this one might need to be always false
        stfData.ExpiryDate = _faker.Date.Soon(5);
        stfData.CreatedBy = Global.TestEmailAddress;
        stfData.DateCreated = DateTime.UtcNow;
        stfData.UpdatedBy = Global.TestEmailAddress;
        stfData.DateUpdated = DateTime.UtcNow;
        stfData.WorkflowStateId = _faker.Random.Int(1, 5); //This should use a lookup from TaskFormData
        stfData.Comment = _faker.Lorem.Sentence();
        stfData.Version = 1;
        stfData.IsDeleted = false;
        stfData.DisplayDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
        stfData.ArchiveComment = string.Empty; //Manually set

        return stfData;
    }
}