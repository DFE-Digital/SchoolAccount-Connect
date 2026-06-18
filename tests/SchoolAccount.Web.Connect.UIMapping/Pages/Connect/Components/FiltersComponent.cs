using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.Utils;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect.Components;

public class FiltersComponent(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    public ILocator ShowFilters => PageContext.Locator("span").Filter(new() { HasText = "Show all sections" });
    public ILocator HideFilters => PageContext.Locator("span").Filter(new() { HasText = "Hide all sections" });
    public ILocator SixteenToNineteenFilter => PageContext.LocatorByTestId("filters-phaseOfEducation-26");
    public ILocator AllThroughFilter => PageContext.LocatorByTestId("filters-phaseOfEducation-20");
    public ILocator MiddlePrimaryFilter => PageContext.LocatorByTestId("filters-phaseOfEducation-23");
    public ILocator MiddleSecondaryFilter => PageContext.LocatorByTestId("filters-phaseOfEducation-24");
    public ILocator NurseryFilter => PageContext.LocatorByTestId("filters-phaseOfEducation-21");
    public ILocator PrimaryFilter => PageContext.LocatorByTestId("filters-phaseOfEducation-22");
    public ILocator SecondaryFilter => PageContext.LocatorByTestId("filters-phaseOfEducation-25");
    public ILocator FinanceFilter => PageContext.LocatorByTestId("filters-category-4");
    public ILocator FundingFilter => PageContext.LocatorByTestId("filters-category-5");
    public ILocator ProcurementFilter => PageContext.LocatorByTestId("filters-category-11");
    public ILocator StaffHRFilter => PageContext.LocatorByTestId("filters-category-7");
    public ILocator AcademyTrustHandBookFilter => PageContext.LocatorByTestId("filters-category-1");
    public ILocator PupilsFilter => PageContext.LocatorByTestId("filters-category-12");
    public ILocator ComplianceFilter => PageContext.LocatorByTestId("filters-category-Compliance");
    public ILocator ApplyFiltersButton => PageContext.LocatorByTestId("submit-button");

    public async Task ApplyFilterAsync(string filterName)
    {
        ILocator filterLocator = filterName.ToLower() switch
        {
            "16 to 19" => SixteenToNineteenFilter,
            "all-through" => AllThroughFilter,
            "middle primary" => MiddlePrimaryFilter,
            "middle secondary" => MiddleSecondaryFilter,
            "nursery" => NurseryFilter,
            "primary" => PrimaryFilter,
            "secondary" => SecondaryFilter,
            "finance" => FinanceFilter,
            "funding" => FundingFilter,
            "procurement" => ProcurementFilter,
            "staff and hr" => StaffHRFilter,
            "academy trust handbook" => AcademyTrustHandBookFilter,
            "pupils" => PupilsFilter,
            _ => throw new ArgumentException($"Unknown filter name: {filterName}")
        };

        await filterLocator.CheckAsync();
        await ApplyFiltersButton.ClickAsync();
    }
}