using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.Utils;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect.Components.Base;

public class GiveFeedbackComponent(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    private string _feedbackButtonText = "Give your feedback (opens in a new tab)";
    public ILocator FeedbackButton => PageContext.AnchorLocatorByText(_feedbackButtonText);
}