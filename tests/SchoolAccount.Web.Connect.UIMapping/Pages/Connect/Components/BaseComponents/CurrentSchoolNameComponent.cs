using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.Utils;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect.Components.Base;

public class CurrentSchoolNameComponent(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    private ILocator _currentSchoolName => PageContext.SpanLocatorByClass("dfe-connect-header__school");

    public async Task<string> GetCurrentSchoolNameAsync()
    {
        return await _currentSchoolName.InnerTextAsync();
    }
}