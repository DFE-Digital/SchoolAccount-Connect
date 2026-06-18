using Microsoft.Playwright;

namespace PlaywrightTests.DfE.UIMapping.Utils;

public static class TeamHelpers
{
    /// <summary>
    /// This helper enters and selects the option for the team name.
    /// </summary>
    /// <param name="service">This now represents the Team, but is still called service in the UI.</param>
    /// <param name="pageContext">The current page context.</param>
    /// <param name="teamName">The team name to be entered.</param>
    /// <remarks>When (if) the UI is updated, the service field and the first option will need to be updated.</remarks>
    public static async Task EnterLinkedTeam(ILocator service, IPage pageContext, string teamName, string optionId)
    {
        /* 
         * This method one has a try catch in it because sometimes the drop down
         * just doesn't load the first time so this should make the suite more stable.
         * 
         */
        int loops = 0;
        while (loops < 3)
        {
            try
            {
                await service.ClearAsync();
                await service.PressSequentiallyAsync(teamName);

                // Wait for the first option to appear and click it.
                var firstOption = pageContext.Locator($"option[id='{optionId}']");
                await firstOption.WaitForAsync();
                await firstOption.ClickAsync();

                return;
            }
            catch
            {
                loops++;
            }
        }

        throw new InvalidOperationException("Failed to enter linked service.");
    }
}
