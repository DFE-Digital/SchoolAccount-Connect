using Bogus;
using PlaywrightTests.GlobalConstants.Global;

namespace PlaywrightTests.DfE.UIMapping.Forms;

//This class uses FormHelpers as Task and SubTask share some helpers.

// To use the class use TaskFormData newTaskForm = TaskFormData.GenerateRandomData();
// Then set individual values as required
public partial class TaskFormData : FormDataBase
{
    private TaskFormData() { }

    public static TaskFormData GenerateRandomData(RandomGeneratorOptions options = RandomGeneratorOptions.None)
    {
        var tfData = new TaskFormData();
        //IsDeleted Must be manually set by the user
        //IsLatestVersion Must be manually set by the user

        // Generate database-mapped fields for Task
        tfData.TaskReferenceNo = $"TASK-{_faker.Random.AlphaNumeric(8).ToUpper()}";

        tfData.TaskName = $"{_faker.Commerce.ProductName()} - {_faker.Random.Words(2)}";

        tfData.TaskDescription = _faker.Lorem.Paragraph();

        tfData.PublishComment = _faker.Lorem.Sentence();

        tfData.ArchiveComment = string.Empty; // Usually empty unless archived

        tfData.CreatedBy = Global.TestEmailAddress;

        tfData.UpdatedBy = Global.TestEmailAddress;

        var requirements = FormHelpers.RequirementIdMapping.Keys.ToArray();
        var selectedRequirement = _faker.PickRandom(requirements);
        tfData.RequirementId = FormHelpers.GetRequirementId(selectedRequirement);

        tfData.WorkflowStateId = _faker.Random.Int(1, 3);

        tfData.Version = _faker.Random.Int(1, 5);
        
        tfData.TeamId = _faker.Random.Int(1, 10);

        return tfData;
    }
}