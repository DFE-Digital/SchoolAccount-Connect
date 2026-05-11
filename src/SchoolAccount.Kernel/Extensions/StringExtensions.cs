namespace SchoolAccount.Kernel.Extensions;

public static class StringExtensions
{
    public static int? ToIntOrDefault(this string? str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }
        
        return int.TryParse(str, out var result) ? result : null;
    }

    public static int ToIntOrDefault(this string? str, int defaultValue)
    {
        return str.ToIntOrDefault() ?? defaultValue;
    }
}