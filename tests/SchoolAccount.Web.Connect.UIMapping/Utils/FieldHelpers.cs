using Microsoft.Playwright;

namespace PlaywrightTests.DfE.UIMapping.Utils;

public static class FieldHelpers
{
    public static async Task EnterLinkedData(ILocator field, IPage pageContext, string text, int retries = 3)
    {
        /* 
         * This method one has a try catch in it because sometimes the drop down
         * just doesn't load the first time so this should make the suite more stable.
         * 
         */
        string? fieldId = await field.GetAttributeAsync("id");
        if (string.IsNullOrWhiteSpace(fieldId))
        {
            throw new InvalidOperationException("Cannot infer id of options field from field locator.");
        }

        string firstOptionId = $"{fieldId}-options-0";
        int loops = 0;
        while (loops < retries)
        {
            try
            {
                await field.ClearAsync();
                await field.PressSequentiallyAsync(text);

                // Wait for the first option to appear and click it.
                var firstOption = pageContext.Locator($"option[id='{firstOptionId}']");
                await firstOption.WaitForAsync();
                await firstOption.ClickAsync();

                return;
            }
            catch
            {
                loops++;
            }
        }

        throw new InvalidOperationException("Failed to enter linked data.");
    }
}
