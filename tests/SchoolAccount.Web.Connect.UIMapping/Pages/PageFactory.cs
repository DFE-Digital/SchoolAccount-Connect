using Microsoft.Playwright;
using PlaywrightTests.DfE.UIMapping.Pages.Connect;
using PlaywrightTests.DfE.UIMapping.Pages.Resources;
using PlaywrightTests.DfE.UIMapping.Pages.Services;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages;

/// Factory class for creating page objects
/// Provides a central place to instantiate and manage page objects
public class PageFactory
{
    private readonly IPage _page;
    private readonly IRunContext _runContext;

    public PageFactory(IPage page, IRunContext runContext)
    {
        _page = page;
        _runContext = runContext;
    }

    public LinksBarPage GetLinksBarPage(ILocator? containerElement = null)
    {
        return new LinksBarPage(_page, _runContext, containerElement);
    }

    public ServicesPage GetServicesPage()
    {
        return new ServicesPage(_page, _runContext);
    }

    public ViewServicesPage GetViewServicesPage()
    {
        return new ViewServicesPage(_page, _runContext);
    }

    public ResourcesPage GetResourcesPage()
    {
        return new ResourcesPage(_page, _runContext);
    }

    public ResourceDetailsPage GetResourceDetailsPage()
    {
        return new ResourceDetailsPage(_page, _runContext);
    }

    public AddResourcePage GetAddResourcePage()
    {
        return new AddResourcePage(_page, _runContext);
    }

    public AddServicePage GetAddServicePage()
    {
        return new AddServicePage(_page, _runContext);
    }

    public ConnectHomePage GetConnectHomePage()
    {
        return new ConnectHomePage(_page, _runContext);
    }

    public ConnectTaskDetailsPage GetConnectTaskDetailsPage()
    {
        return new ConnectTaskDetailsPage(_page, _runContext);
    }

    public ConnectCalendarPage GetConnectCalendarPage()
    {
        return new ConnectCalendarPage(_page, _runContext);
    }

    public async Task<string> TakeScreenshotAsync(string filename, bool fullPage = true)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        var screenshotDir = Path.Combine(Directory.GetCurrentDirectory(), "screenshots", "manual");
        
        // Ensure screenshot directory exists
        Directory.CreateDirectory(screenshotDir);
        
        // Create the full filename with timestamp
        var fullFilename = $"{filename}_{timestamp}.png";
        var screenshotPath = Path.Combine(screenshotDir, fullFilename);
        
        try
        {
            await _page.ScreenshotAsync(new PageScreenshotOptions 
            { 
                Path = screenshotPath, 
                FullPage = fullPage 
            });

            _runContext.Log($"Manual screenshot saved: {screenshotPath}");
            return screenshotPath;
        }
        catch (Exception ex)
        {
            _runContext.Log($"Failed to capture manual screenshot: {ex.Message}");
            throw;
        }
    }

    public async Task<string> TakeScreenshotWithDescriptionAsync(string description = "manual_capture", bool fullPage = true)
    {
        // Get the current test method name from the stack trace
        var testName = GetCurrentTestName() ?? "unknown_test";
        var filename = $"{testName}_{description}";
        
        return await TakeScreenshotAsync(filename, fullPage);
    }

    private string? GetCurrentTestName()
    {
        try
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            var frames = stackTrace.GetFrames();
            
            foreach (var frame in frames)
            {
                var method = frame.GetMethod();
                if (method != null && 
                    (method.GetCustomAttributes(typeof(Xunit.FactAttribute), false).Length > 0 ||
                     method.GetCustomAttributes(typeof(Xunit.TheoryAttribute), false).Length > 0))
                {
                    return method.Name;
                }
            }
        }
        catch
        {
            // If there's no test name it shouldn't prevent things running.
        }
        
        return null;
    }
}