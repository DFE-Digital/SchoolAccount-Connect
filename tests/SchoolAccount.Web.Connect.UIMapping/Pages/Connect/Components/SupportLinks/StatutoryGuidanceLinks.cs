using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.Utils;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect.Components.Support;

public class StatutoryGuidanceLinks(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    public ILocator AcademyTrustHandbook => PageContext.AnchorLocatorByText("Academy Trust Handbook");
    public ILocator KeepingChildrenSafe => PageContext.AnchorLocatorByText("Keeping children safe in education");
    public ILocator PublicProcurement => PageContext.AnchorLocatorByText("Public procurement policy");
    public ILocator SchoolAdmissions => PageContext.AnchorLocatorByText("School admissions code");
    public ILocator SENDPractice => PageContext.AnchorLocatorByText("SEND code of practice");
}