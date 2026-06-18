using Microsoft.Playwright;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.DfE.UIMapping.Pages.Connect.Components;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect;

public class ConnectHomePage(IPage page, IRunContext runContext) : ConnectBasePage(page, runContext)
{
    //This is the home page shown when logged into Connect
    public ExploreTasksComponent ExploreTasks { get; } = new ExploreTasksComponent(page, runContext);
    public TasksByDateComponent TasksByDate { get; } = new TasksByDateComponent(page, runContext);
    public FullCalendarOfTasksLink COTLink { get; } = new FullCalendarOfTasksLink(page, runContext);
}