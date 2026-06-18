using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.DfE.UIMapping.Pages.Connect.Components.Base;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect;

public class ConnectBasePage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    public SearchBarComponent SearchBar { get; } = new SearchBarComponent(page, runContext);
    public NavBarComponent NavBar { get; } = new NavBarComponent(page, runContext);
    public GiveFeedbackComponent GiveFeedback { get; } = new GiveFeedbackComponent(page, runContext);
    public CurrentSchoolNameComponent CurrentSchoolName { get; } = new CurrentSchoolNameComponent(page, runContext);
}