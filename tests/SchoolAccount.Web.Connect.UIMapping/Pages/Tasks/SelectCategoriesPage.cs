using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.Kernel.Utils;

namespace PlaywrightTests.DfE.UIMapping.Pages.Tasks;

/// <summary>
/// Page model allowing the categories to be selected.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class SelectCategoriesPage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    private ILocator SaveAndContinueButton => PageContext.ButtonLocatorByText("Save and continue");

    public async Task SelectCategoryByLabelAsync(string label)
    {
        var category = PageContext.GetByLabel(label);
        await category.ClickAsync();
    }

    public async Task ClickSaveAndContinueAsync()
    {
        await SaveAndContinueButton.ClickAsync();
    }
}
