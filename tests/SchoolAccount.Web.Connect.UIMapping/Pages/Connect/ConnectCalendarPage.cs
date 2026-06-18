using Microsoft.Playwright;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.DfE.UIMapping.Pages.Connect.Components;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect;

public class ConnectCalendarPage(IPage page, IRunContext runContext) : ConnectBasePage(page, runContext)
{
    public TasksByDateComponent TasksByDate { get; } = new TasksByDateComponent(page, runContext);
    public FiltersComponent Filters { get; } = new FiltersComponent(page, runContext);
}