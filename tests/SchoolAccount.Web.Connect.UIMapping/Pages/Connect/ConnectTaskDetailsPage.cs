using Microsoft.Playwright;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.DfE.UIMapping.Pages.Connect.Components;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect;

public class ConnectTaskDetailsPage(IPage page, IRunContext runContext) : ConnectBasePage(page, runContext)
{
    public TasksByDateComponent TasksByDate { get; } = new TasksByDateComponent(page, runContext);
    public TaskDetailsComponent TaskDetails { get; } = new TaskDetailsComponent(page, runContext);

}