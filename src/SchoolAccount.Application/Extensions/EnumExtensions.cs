namespace SchoolAccount.Application.Extensions;

public static class EnumExtensions
{
    public static bool HasFlags<T>(this T enumValue, params T[] flags)
        where T : struct, Enum
    {
        return flags.Any(x => enumValue.HasFlag(x));
    }
}
