using Microsoft.Playwright;
using PlaywrightTests.DfE.UIMapping.Pages.Tasks;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.Kernel.Utils;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightTests.DfE.UIMapping.Pages;

/// <summary>
/// Page Object for the Links Bar component containing navigation links
/// Represents the navigation bar with Home, Tasks, Services, and Resources links
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="containerElement"></param>
public class LinksBarPage(IPage page, IRunContext runContext, ILocator? containerElement = null) : BasePage(page, runContext, containerElement)
{
    // Locators for the navigation links
    public ILocator HomeLink => PageContext.Locator("nav a").Filter(new() { HasText = "Home" });
    public ILocator TasksLink => PageContext.Locator("nav a").Filter(new() { HasText = "Tasks" });
    public ILocator ServicesLink => PageContext.Locator("nav a").Filter(new() { HasText = "Services" });
    public ILocator ResourcesLink => PageContext.Locator("nav a").Filter(new() { HasText = "Resources" });
    public ILocator CreateATaskButton => PageContext.ButtonLocatorByText("Create a task");

    public async Task ClickHomeAsync()
    {
        await HomeLink.ClickAsync();
        await WaitForPageLoadAsync();
    }

    public async Task<TasksPage> ClickTasksAsync()
    {
        await TasksLink.ClickAsync();
        await WaitForPageLoadAsync();

        return new TasksPage(PageContext, RunContext);
    }

    public async Task<AddTaskPage> ClickCreateATaskAsync()
    {
        await CreateATaskButton.ClickAsync();
        await WaitForPageLoadAsync();

        return new AddTaskPage(PageContext, RunContext);
    }

    public async Task ClickServicesAsync()
    {
        await ServicesLink.ClickAsync();
        await WaitForPageLoadAsync();
    }

    public async Task ClickResourcesAsync()
    {
        await ResourcesLink.ClickAsync();
        await WaitForPageLoadAsync();
    }

    public async Task VerifyAllLinksVisibleAsync()
    {
        await Expect(HomeLink).ToBeVisibleAsync();
        await Expect(TasksLink).ToBeVisibleAsync();
        await Expect(ServicesLink).ToBeVisibleAsync();
        await Expect(ResourcesLink).ToBeVisibleAsync();
    }

    public async Task VerifyPageTitleAsync()
    {
        await AssertTitle("Manage your DfE Connect data");
    }
}