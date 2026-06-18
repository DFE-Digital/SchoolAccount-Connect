using Microsoft.Playwright;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.DfE.UIMapping.Pages.Connect.Components;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect;

public class ConnectPreviousPage(IPage page, IRunContext runContext) : ConnectBasePage(page, runContext)
{
    public ExploreTasksComponent ExploreTasks { get; } = new ExploreTasksComponent(page, runContext);
    public FiltersComponent Filters { get; } = new FiltersComponent(page, runContext);
    public TasksByDateComponent TasksByDate { get; } = new TasksByDateComponent(page, runContext);
}