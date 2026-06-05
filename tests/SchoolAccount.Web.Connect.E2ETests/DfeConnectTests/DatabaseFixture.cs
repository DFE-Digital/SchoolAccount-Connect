using PlaywrightTests.DfE.Browsers.Config;
using PlaywrightTests.DfE.Infrastructure;
using PlaywrightTests.GlobalConstants.Global;
using PlaywrightTests.GlobalConstants.ConfigTableNames;
using PlaywrightTests.Kernel.Config;
using PlaywrightTests.Kernel.Utils;

namespace PlaywrightTests.DfE.Tests.DfeConnectTests;

public class DatabaseCollectionFixture : IAsyncLifetime
{
    private Database _database = null!;

    public async ValueTask InitializeAsync()
    {
        var config = ConfigLoader<ConnectRunConfig>.Load(
            TestDirectories.GetProjectDirectory(),
            ConfigFilePath.RelativeConfigFileName);
        _database = new Database(config);
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        string createdByUser = $"CreatedBy = '{Global.TestEmailAddress}'";
        string deleteTagsSourceMappingSql = SQLHelper.GenerateDeleteScript(ConfigTableNames.TagsSourceMappingTable, $"EntityId IN (SELECT Id FROM dbo.SubTask WHERE {createdByUser})");
        await _database.DeleteAsync(deleteTagsSourceMappingSql);
        string deleteTypeTaskMappingSql = SQLHelper.GenerateDeleteScript(ConfigTableNames.TypeTaskMappingTable, $"TaskId IN (SELECT Id FROM dbo.Task WHERE {createdByUser})");
        await _database.DeleteAsync(deleteTypeTaskMappingSql);
        // Cleanup Task and SubTask entries
        string deleteSubTaskSql = SQLHelper.GenerateDeleteScript(ConfigTableNames.SubTaskTable, createdByUser);
        await _database.DeleteAsync(deleteSubTaskSql);
        string deleteTaskSql = SQLHelper.GenerateDeleteScript(ConfigTableNames.TaskTable, createdByUser);
        await _database.DeleteAsync(deleteTaskSql);
    }
}

//Any collection with DatabaseCollection will have this run afterwards.
[CollectionDefinition("DatabaseCollection")]
public class DatabaseCollection : ICollectionFixture<DatabaseCollectionFixture> { }