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
using PlaywrightTests.Kernel.Config;

namespace PlaywrightTests.DfE.Tests;

public class ComponentTests(ComponentClassFixture classFixture, ITestOutputHelper testOutputHelper)
    : MultiBrowserTestBase<ConfigBase>(classFixture, testOutputHelper), IClassFixture<ComponentClassFixture>
{
    private BrowserSessionBase<ConfigBase> _browserSession = null!;
    private PageFactory _pageFactory = null!;
    private Database _database = null!;

    private ConnectHomePage _connectHomePage = null!;


    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();

        _browserSession = GetBrowserSession<ComponentBrowserSession>();
        _pageFactory = new PageFactory(_browserSession.CurrentPageContext, FixtureContext);
        _connectHomePage = _pageFactory.GetConnectHomePage();
    }

    private async Task<int> CreateTaskReturnId(TaskFormData taskFormData)
    {
        var insertSql = SQLHelper.GenerateInsertScript(taskFormData, typeof(TaskTable), ConfigTableNames.TaskTable, true);
        var taskIdResults = await _database.ExecuteScalarAsync<int>(insertSql, taskFormData);
        FixtureContext.Log($"Inserted new task with name: {taskFormData.TaskName} for team ID: {taskFormData.TeamId}");

        FixtureContext.Log($"Retrieved Task ID: {taskIdResults} for task with name: {taskFormData.TaskName}");
        return (int)taskIdResults;
    }

    //compliance category - 1
    //Compliance = Type 2
    [Fact]
    [Trait("Category", "ComponentDemo")]
    public async Task ComponentDemoTest()
    {
        _connectHomePage.GoToGoogle();
        await Task.Delay(15000, TestContext.Current.CancellationToken);
    }

}