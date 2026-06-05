using System.Globalization;
using PlaywrightTests.DfE.Browsers.BrowserSessions;
using PlaywrightTests.DfE.Browsers.Config;
using PlaywrightTests.DfE.Browsers.TestFixtures;
using PlaywrightTests.GlobalConstants.ConfigTableNames;
using PlaywrightTests.DfE.Infrastructure;
using PlaywrightTests.DfE.Infrastructure.TableDefinitions;
using PlaywrightTests.DfE.Tests.Utils;
using PlaywrightTests.DfE.UIMapping.Forms;
using PlaywrightTests.DfE.UIMapping.Pages;
using PlaywrightTests.DfE.UIMapping.Pages.Connect;
using PlaywrightTests.DfE.UIMapping.Pages.Connect.Components;
using PlaywrightTests.Kernel.BrowserSessions;
using PlaywrightTests.Kernel.TestBases;

namespace PlaywrightTests.DfE.Tests;

[Collection("DatabaseCollection")]
public class BasicConnectTests(ConnectClassFixture classFixture, ITestOutputHelper testOutputHelper)
    : MultiBrowserTestBase<ConnectRunConfig>(classFixture, testOutputHelper), IClassFixture<ConnectClassFixture>
{
    private BrowserSessionBase<ConnectRunConfig> _browserSession = null!;
    private PageFactory _pageFactory = null!;
    private Database _database = null!;
    private string _randomTaskId = null!;
    private ConnectHomePage _connectHomePage = null!;
    private ConnectTaskDetailsPage _connectTaskDetailsPage = null!;
    private ConnectCalendarPage _connectCalendarPage = null!;

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();

        _browserSession = GetBrowserSession<ConnectBrowserSession>();
        _pageFactory = new PageFactory(_browserSession.CurrentPageContext, FixtureContext);
        _connectHomePage = _pageFactory.GetConnectHomePage();
        _connectTaskDetailsPage = _pageFactory.GetConnectTaskDetailsPage();
        _connectCalendarPage = _pageFactory.GetConnectCalendarPage();


        _database = new Database(FixtureContext.Config);
        _randomTaskId = await _database.GetRandomField(ConfigTableNames.TaskTable, "Id");
        _randomTaskId = 1146.ToString(CultureInfo.InvariantCulture);
    }

    private async Task<int> CreateTaskReturnId(TaskFormData taskFormData)
    {
        var insertSql = SQLHelper.GenerateInsertScript(taskFormData, typeof(TaskTable), ConfigTableNames.TaskTable, true);
        var taskIdResults = await _database.ExecuteScalarAsync<int>(insertSql, taskFormData);
        FixtureContext.Log($"Inserted new task with name: {taskFormData.TaskName} for team ID: {taskFormData.TeamId}");

        FixtureContext.Log($"Retrieved Task ID: {taskIdResults} for task with name: {taskFormData.TaskName}");
        return (int)taskIdResults;
    }

    private async Task<long> CreateSubTaskReturnId(SubTaskFormData subTaskFormData)
    {
        var insertSubTaskSql = SQLHelper.GenerateInsertScript(subTaskFormData, typeof(SubTaskTable), ConfigTableNames.SubTaskTable, true);
        var subtaskIdResults = await _database.ExecuteScalarAsync<long>(insertSubTaskSql, subTaskFormData);
        FixtureContext.Log($"Retrieved SubTask ID: {subtaskIdResults} for subtask with name: {subTaskFormData.SubTaskName}");
        return subtaskIdResults;
    }
    
    private async void CreateTagsSourceMappingTable(long entityId, int sourceId, int tagId)
    {
        TagsSourceMappingTableData tagsSourceMappingData = new TagsSourceMappingTableData
        {
            EntityId = entityId,
            SourceId = sourceId,
            TagId = tagId
        };
        var insertTagsSourceMappingSql = SQLHelper.GenerateInsertScript(tagsSourceMappingData, typeof(TagsSourceMappingTable), ConfigTableNames.TagsSourceMappingTable);
        await _database.InsertAsync(insertTagsSourceMappingSql, tagsSourceMappingData);
    }

    private async void CreateTypeTaskMappingTable(long taskId, int typeId)
    {
        TypeTaskMappingData typeTaskMappingData = new TypeTaskMappingData
        {
            TaskId = taskId,
            TypeId = typeId
        };
        var insertTypeTaskMappingSql = SQLHelper.GenerateInsertScript(typeTaskMappingData, typeof(TypeTaskMappingData), ConfigTableNames.TypeTaskMappingTable);
        await _database.InsertAsync(insertTypeTaskMappingSql, typeTaskMappingData);
    }

    [Fact]
    [Trait("Category", "Connect_Task")]
    [Trait("Category", "Pipeline")]
    public async Task ConnectDBValidationTestTasks()
    {
        //Database Setup for new Team to link Resource to.
        (int teamId, string teamName) = await TeamFactory.GenerateAndInsertTeam(_database);
        //From here we can generate a new resource entry linked to the new Team

        var taskFormData = TaskFormData.GenerateRandomData();
        taskFormData.TeamId = teamId;

        // Print out each property with its value
        FixtureContext.Log("=== Generated TaskFormData ===");
        FixtureContext.Log($"TaskReferenceNo: {taskFormData.TaskReferenceNo}");
        FixtureContext.Log($"TaskName: {taskFormData.TaskName}");
        FixtureContext.Log($"TaskDescription: {taskFormData.TaskDescription}");
        FixtureContext.Log($"ServiceId: {taskFormData.ServiceId}");
        FixtureContext.Log($"PublishDate: {taskFormData.PublishDate}");
        FixtureContext.Log($"RequirementId: {taskFormData.RequirementId}");
        FixtureContext.Log($"WorkflowStateId: {taskFormData.WorkflowStateId}");
        FixtureContext.Log($"IsDeleted: {taskFormData.IsDeleted}");
        FixtureContext.Log($"PublishComment: {taskFormData.PublishComment}");
        FixtureContext.Log($"ArchiveComment: {taskFormData.ArchiveComment}");
        FixtureContext.Log($"CreatedBy: {taskFormData.CreatedBy}");
        FixtureContext.Log($"DateCreated: {taskFormData.DateCreated}");
        FixtureContext.Log($"UpdatedBy: {taskFormData.UpdatedBy}");
        FixtureContext.Log($"DateUpdated: {taskFormData.DateUpdated}");
        FixtureContext.Log($"TeamId: {taskFormData.TeamId}");
        FixtureContext.Log($"Version: {taskFormData.Version}");
        FixtureContext.Log($"IsLatestVersion: {taskFormData.IsLatestVersion}");
        FixtureContext.Log("================================");

        var insertSql = SQLHelper.GenerateInsertScript(taskFormData, typeof(TaskTable), ConfigTableNames.TaskTable);
        await _database.InsertAsync(insertSql, taskFormData);
        FixtureContext.Log($"Inserted new task with name: {taskFormData.TaskName} for team ID: {taskFormData.TeamId}");
    }

    [Fact]
    [Trait("Category", "Connect")]
    [Trait("Category", "Pipeline")]
    public async Task ConnectDBValidationTestSubTasks()
    {
        //Database Setup for new Team to link Resource to.
        (int teamId, string teamName) = await TeamFactory.GenerateAndInsertTeam(_database);
        //From here we can generate a new resource entry linked to the new Team

        var subTaskFormData = SubTaskFormData.GenerateRandomData();
        subTaskFormData.TaskId = int.Parse(_randomTaskId, CultureInfo.InvariantCulture);
        subTaskFormData.TeamId = teamId;

        // Print out each property with its value
        FixtureContext.Log("=== Generated SubTaskFormData ===");
        FixtureContext.Log($"ServiceId: {subTaskFormData.ServiceId}");
        FixtureContext.Log($"SubTaskReferenceNo: {subTaskFormData.SubTaskReferenceNo}");
        FixtureContext.Log($"SubTaskName: {subTaskFormData.SubTaskName}");
        FixtureContext.Log($"SubTaskDescription: {subTaskFormData.SubTaskDescription}");
        FixtureContext.Log($"DigitalTaskLink: {subTaskFormData.DigitalTaskLink}");
        FixtureContext.Log($"RequirementId: {subTaskFormData.RequirementId}");
        FixtureContext.Log($"StartDate: {subTaskFormData.StartDate}");
        FixtureContext.Log($"StartDateIsExact: {subTaskFormData.StartDateIsExact}");
        FixtureContext.Log($"DueDate: {subTaskFormData.DueDate}");
        FixtureContext.Log($"DueDateIsExact: {subTaskFormData.DueDateIsExact}");
        FixtureContext.Log($"ExpiryDate: {subTaskFormData.ExpiryDate}");
        FixtureContext.Log($"CreatedBy: {subTaskFormData.CreatedBy}");
        FixtureContext.Log($"DateCreated: {subTaskFormData.DateCreated}");
        FixtureContext.Log($"UpdatedBy: {subTaskFormData.UpdatedBy}");
        FixtureContext.Log($"DateUpdated: {subTaskFormData.DateUpdated}");
        FixtureContext.Log($"WorkflowStateId: {subTaskFormData.WorkflowStateId}");
        FixtureContext.Log($"Comment: {subTaskFormData.Comment}");
        FixtureContext.Log($"Version: {subTaskFormData.Version}");
        FixtureContext.Log($"IsDeleted: {subTaskFormData.IsDeleted}");
        FixtureContext.Log($"DisplayDate: {subTaskFormData.DisplayDate}");
        FixtureContext.Log($"ArchiveComment: {subTaskFormData.ArchiveComment}");
        FixtureContext.Log($"TeamId: {subTaskFormData.TeamId}");
        FixtureContext.Log("================================");

        var insertSql = SQLHelper.GenerateInsertScript(subTaskFormData, typeof(SubTaskTable), ConfigTableNames.SubTaskTable);
        await _database.InsertAsync(insertSql, subTaskFormData);
        FixtureContext.Log($"Inserted new sub-task with name: {subTaskFormData.SubTaskName} for team ID: {subTaskFormData.TeamId}");
    }

    [Fact]
    [Trait("Category", "Connect")]
    [Trait("Category", "Pipeline")]
    public async Task ConnectDBValidationTestTeam()
    {
        //Database Setup for new Team to link Resource to.
        TeamFormData teamFormData = await TeamFactory.GenerateTeam();
        //From here we can generate a new resource entry linked to the new Team

        // Print out each property with its value
        FixtureContext.Log("=== Generated TeamFormData ===");
        FixtureContext.Log($"TeamName: {teamFormData.TeamName}");
        FixtureContext.Log($"Acronym: {teamFormData.Acronym}");
        FixtureContext.Log($"TeamDescription: {teamFormData.TeamDescription}");
        FixtureContext.Log($"GroupId: {teamFormData.GroupId}");
        FixtureContext.Log($"DirectorateId: {teamFormData.DirectorateId}");
        FixtureContext.Log($"DeputyDirector: {teamFormData.DeputyDirector}");
        FixtureContext.Log($"TeamInboxEmail: {teamFormData.TeamInboxEmail}");
        FixtureContext.Log($"TeamOwnerNames: {teamFormData.TeamOwnerNames}");
        FixtureContext.Log($"WorkflowStateId: {teamFormData.WorkflowStateId}");
        FixtureContext.Log($"CreatedBy: {teamFormData.CreatedBy}");
        FixtureContext.Log($"DateCreated: {teamFormData.DateCreated}");
        FixtureContext.Log($"UpdatedBy: {teamFormData.UpdatedBy}");
        FixtureContext.Log($"DateUpdated: {teamFormData.DateUpdated}");
        FixtureContext.Log($"IsDeleted: {teamFormData.IsDeleted}");
        FixtureContext.Log($"DigitalServiceLink: {teamFormData.DigitalServiceLink}");
        FixtureContext.Log($"TeamStatusId: {teamFormData.TeamStatusId}");
        FixtureContext.Log($"DueToDecommissionDate: {teamFormData.DueToDecommissionDate}");
        FixtureContext.Log($"IsExactDecommissionDate: {teamFormData.IsExactDecommissionDate}");
        FixtureContext.Log($"SupportLevelId: {teamFormData.SupportLevelId}");
        FixtureContext.Log("================================");

        var insertSql = SQLHelper.GenerateInsertScript(teamFormData, typeof(TeamTable), ConfigTableNames.TeamTable);
        await _database.InsertAsync(insertSql, teamFormData);
        FixtureContext.Log($"Inserted new team with name: {teamFormData.TeamName}");
    }


    [Fact]
    [Trait("Category", "Connect_UI")]
    public async Task ConnectUIValidation()
    {
        await _connectHomePage.ExploreTasks.FinanceLink.ClickAsync();
    }

    [Fact]
    [Trait("Category", "Connect_UI")]
    [Trait("Category", "Filter_Check")]
    public async Task ConnectFiltersValidation1()
    {
        string TaskName = "COT MULTI 2";
        await _connectHomePage.COTLink.Link.ClickAsync();
        await _connectCalendarPage.Filters.ShowFilters.ClickAsync();
        //We need to force the click for some reason. An element that does not overlay is 'intercepting'
        await _connectCalendarPage.Filters.AllThroughFilter.ClickAsync(new() { Force = true });
        await _connectCalendarPage.Filters.ApplyFiltersButton.ClickAsync();
        await _connectCalendarPage.TasksByDate.ClickTaskByNameAsync(TaskName);
        // var taskTitle = await _connectTaskDetailsPage.TaskDetails.TaskTitle.TextContentAsync();
        // Assert.Contains(TaskName, taskTitle ?? string.Empty);
    }

    [Fact]
    [Trait("Category", "Connect_UI")]
    [Trait("Category", "Filter_Check")]
    public async Task ConnectFiltersFinanceValidation1()
    {
        await _connectHomePage.COTLink.Link.ClickAsync();
        await _connectCalendarPage.Filters.ShowFilters.ClickAsync();
        //We need to force the click for some reason. An element that does not overlay is 'intercepting'
        await _connectCalendarPage.Filters.FinanceFilter.ClickAsync(new() { Force = true });
        await _connectCalendarPage.Filters.ApplyFiltersButton.ClickAsync();
        List<TaskItem> taskItems = await _connectCalendarPage.TasksByDate.GetAllTaskItemsAsync();
        //Check no values are present in the taskItems list that contain the TaskName when the Finance filter is applied
        // Assert.Empty(taskItems);
    }

    //Test to create a new DB Entry and then validate it has appeared.
    [Fact]
    [Trait("Category", "Connect_UI")]
    [Trait("Category", "Filter_DB")]
    public async Task ConnectFiltersDBEntry()
    {
        (int teamId, string teamName) = await TeamFactory.GenerateAndInsertTeam(_database);
        var taskFormData = TaskFormData.GenerateRandomData();
        taskFormData.TeamId = teamId;
        taskFormData.RequirementId = 1; // Requires a Mandatory Task
        taskFormData.WorkflowStateId = 3;
        taskFormData.IsLatestVersion = true;
        int taskId = await CreateTaskReturnId(taskFormData);

        var subTaskFormData = SubTaskFormData.GenerateRandomData();
        subTaskFormData.TaskId = taskId;
        subTaskFormData.TeamId = taskFormData.TeamId;
        subTaskFormData.WorkflowStateId = 3; // Published
        subTaskFormData.RequirementId = 1; // Requires a Mandatory Task, to ensure it appears when filters are applied.
        long subTaskId = await CreateSubTaskReturnId(subTaskFormData);

        CreateTagsSourceMappingTable(subTaskId, 3, 27); 

        FixtureContext.Log($"Inserted new sub-task with name: {subTaskFormData.SubTaskName} for task ID: {subTaskFormData.TaskId}");

        //Frontend Checks
        await _connectHomePage.COTLink.Link.ClickAsync();
        await _connectCalendarPage.TasksByDate.ClickTaskByNameAsync(taskFormData.TaskName);
        var taskTitle = await _connectTaskDetailsPage.TaskDetails.TaskTitle.TextContentAsync();
        Assert.Contains(taskFormData.TaskName, taskTitle ?? string.Empty, StringComparison.Ordinal);
    }

    /// Negative Tests to validate that entries that SHOULDN'T appear, don't.
    [Fact]
    [Trait("Category", "Connect_UI")]
    [Trait("Category", "Filter_DB")]
    public async Task ConnectFiltersDBEntryNoReqId()
    {
        (int teamId, string teamName) = await TeamFactory.GenerateAndInsertTeam(_database);

        var taskFormData = TaskFormData.GenerateRandomData();
        taskFormData.TeamId = teamId;
        taskFormData.RequirementId = 2; // Non Mandatory Task (shouldn't show)
        taskFormData.WorkflowStateId = 3;
        taskFormData.IsLatestVersion = true;
        int taskId = await CreateTaskReturnId(taskFormData);

        var subTaskFormData = SubTaskFormData.GenerateRandomData();
        subTaskFormData.TaskId = taskId;
        subTaskFormData.TeamId = taskFormData.TeamId;
        subTaskFormData.WorkflowStateId = 3; // Published
        subTaskFormData.RequirementId = 1; // Requires a Mandatory Task, to ensure it appears when filters are applied.
        long subTaskId = await CreateSubTaskReturnId(subTaskFormData);

        CreateTagsSourceMappingTable(subTaskId, 3, 27);

        FixtureContext.Log($"Inserted new sub-task with name: {subTaskFormData.SubTaskName} for task ID: {subTaskFormData.TaskId}");

        //Frontend Checks
        await _connectHomePage.COTLink.Link.ClickAsync();
        List<TaskItem> taskItems = await _connectCalendarPage.TasksByDate.GetAllTaskItemsAsync();
        Assert.True(taskItems == null || taskItems.All(t => t.TaskName != taskFormData.TaskName), 
            $"Task with name '{taskFormData.TaskName}' was unexpectedly found in the task list.");
    }

    //Create with incorrect RequirementId to show it doesn't show
    [Fact]
    [Trait("Category", "Connect_UI")]
    [Trait("Category", "Filter_DB")]
    public async Task ConnectFiltersDBEntryNoSubTask()
    {
        (int teamId, string teamName) = await TeamFactory.GenerateAndInsertTeam(_database);
        var taskFormData = TaskFormData.GenerateRandomData();
        taskFormData.TeamId = teamId;
        taskFormData.RequirementId = 1; // Non Mandatory Task (shouldn't show)
        taskFormData.WorkflowStateId = 3;
        taskFormData.IsLatestVersion = true;
        int taskId = await CreateTaskReturnId(taskFormData);

        //Frontend Checks
        await _connectHomePage.COTLink.Link.ClickAsync();
        List<TaskItem> taskItems = await _connectCalendarPage.TasksByDate.GetAllTaskItemsAsync();
        Assert.True(taskItems == null || taskItems.All(t => t.TaskName != taskFormData.TaskName),
            $"Task with name '{taskFormData.TaskName}' was unexpectedly found in the task list.");
    }

    //Filters Tests
    //These need to have extra source mappings
    [Fact]
    [Trait("Category", "Connect_UI")]
    [Trait("Category", "Filter_DB")]
    public async Task Connect16to19FiltersDBEntry()
    {
        (int teamId, string teamName) = await TeamFactory.GenerateAndInsertTeam(_database);
        var taskFormData = TaskFormData.GenerateRandomData();
        taskFormData.TeamId = teamId;
        taskFormData.RequirementId = 1; // Requires a Mandatory Task
        taskFormData.WorkflowStateId = 3;
        taskFormData.IsLatestVersion = true;
        int taskId = await CreateTaskReturnId(taskFormData);

        var subTaskFormData = SubTaskFormData.GenerateRandomData();
        subTaskFormData.TaskId = taskId;
        subTaskFormData.TeamId = taskFormData.TeamId;
        subTaskFormData.WorkflowStateId = 3; // Published
        subTaskFormData.RequirementId = 1; // Requires a Mandatory Task, to ensure it appears when filters are applied.
        long subTaskId = await CreateSubTaskReturnId(subTaskFormData);

        CreateTagsSourceMappingTable(subTaskId, 3, 27);
        CreateTagsSourceMappingTable(subTaskId, 3, 26);

        FixtureContext.Log($"Inserted new sub-task with name: {subTaskFormData.SubTaskName} for task ID: {subTaskFormData.TaskId}");

        //Frontend Checks
        await _connectHomePage.COTLink.Link.ClickAsync();
        await _connectCalendarPage.Filters.ShowFilters.ClickAsync();
        await _connectCalendarPage.Filters.SixteenToNineteenFilter.ClickAsync(new() { Force = true });
        await _connectCalendarPage.Filters.ApplyFiltersButton.ClickAsync();
        await _connectCalendarPage.TasksByDate.ClickTaskByNameAsync(taskFormData.TaskName);
        var taskTitle = await _connectTaskDetailsPage.TaskDetails.TaskTitle.TextContentAsync();
        Assert.Contains(taskFormData.TaskName, taskTitle ?? string.Empty, StringComparison.Ordinal);
    }

    [Fact]
    [Trait("Category", "Connect_UI")]
    [Trait("Category", "Filter_DB")]
    public async Task ConnectMiddlePrimaryFiltersDBEntry()
    {
        (int teamId, string teamName) = await TeamFactory.GenerateAndInsertTeam(_database);
        var taskFormData = TaskFormData.GenerateRandomData();
        taskFormData.TeamId = teamId;
        taskFormData.RequirementId = 1; // Requires a Mandatory Task
        taskFormData.WorkflowStateId = 3;
        taskFormData.IsLatestVersion = true;
        int taskId = await CreateTaskReturnId(taskFormData);

        var subTaskFormData = SubTaskFormData.GenerateRandomData();
        subTaskFormData.TaskId = taskId;
        subTaskFormData.TeamId = taskFormData.TeamId;
        subTaskFormData.WorkflowStateId = 3; // Published
        subTaskFormData.RequirementId = 1; // Requires a Mandatory Task, to ensure it appears when filters are applied.
        long subTaskId = await CreateSubTaskReturnId(subTaskFormData);

        CreateTagsSourceMappingTable(subTaskId, 3, 27);
        CreateTagsSourceMappingTable(subTaskId, 3, 23);

        FixtureContext.Log($"Inserted new sub-task with name: {subTaskFormData.SubTaskName} for task ID: {subTaskFormData.TaskId}");

        //Frontend Checks
        await _connectHomePage.COTLink.Link.ClickAsync();
        await _connectCalendarPage.Filters.ShowFilters.ClickAsync();
        await _connectCalendarPage.Filters.MiddlePrimaryFilter.ClickAsync(new() { Force = true });
        await _connectCalendarPage.Filters.ApplyFiltersButton.ClickAsync();
        await _connectCalendarPage.TasksByDate.ClickTaskByNameAsync(taskFormData.TaskName);
        var taskTitle = await _connectTaskDetailsPage.TaskDetails.TaskTitle.TextContentAsync();
        Assert.Contains(taskFormData.TaskName, taskTitle ?? string.Empty, StringComparison.Ordinal);
    }
    //compliance category - 1
    //Compliance = Type 2
    [Fact]
    [Trait("Category", "Connect_UI")]
    [Trait("Category", "Filter_DB")]
    public async Task ConnectComplianceFiltersDBEntry()
    {
        (int teamId, string teamName) = await TeamFactory.GenerateAndInsertTeam(_database);
        var taskFormData = TaskFormData.GenerateRandomData();
        taskFormData.TeamId = teamId;
        taskFormData.RequirementId = 1; // Requires a Mandatory Task
        taskFormData.WorkflowStateId = 3;
        taskFormData.IsLatestVersion = true;
        int taskId = await CreateTaskReturnId(taskFormData);

        var subTaskFormData = SubTaskFormData.GenerateRandomData();
        subTaskFormData.TaskId = taskId;
        subTaskFormData.TeamId = taskFormData.TeamId;
        subTaskFormData.WorkflowStateId = 3; // Published
        subTaskFormData.RequirementId = 1; // Requires a Mandatory Task, to ensure it appears when filters are applied.
        long subTaskId = await CreateSubTaskReturnId(subTaskFormData);

        CreateTagsSourceMappingTable(subTaskId, 3, 27);
        CreateTagsSourceMappingTable(subTaskId, 3, 1); 
        CreateTypeTaskMappingTable(taskId, 2); // Link the Task to the Compliance category as well, to ensure it appears when filters are applied.

        FixtureContext.Log($"Inserted new sub-task with name: {subTaskFormData.SubTaskName} for task ID: {subTaskFormData.TaskId}");

        //Frontend Checks
        await _connectHomePage.COTLink.Link.ClickAsync();
        await _connectCalendarPage.Filters.ShowFilters.ClickAsync();
        await _connectCalendarPage.Filters.ComplianceFilter.ClickAsync(new() { Force = true });
        await _connectCalendarPage.Filters.ApplyFiltersButton.ClickAsync();
        await _connectCalendarPage.TasksByDate.ClickTaskByNameAsync(taskFormData.TaskName);
        var taskTitle = await _connectTaskDetailsPage.TaskDetails.TaskTitle.TextContentAsync();
        Assert.Contains(taskFormData.TaskName, taskTitle ?? string.Empty, StringComparison.Ordinal);
    }

}