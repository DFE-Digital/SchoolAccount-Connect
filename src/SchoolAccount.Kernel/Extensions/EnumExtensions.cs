using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace SchoolAccount.Kernel.Extensions;

public static class EnumExtensions
{
    public static T ParseFlexible<T>(string value) where T : struct, Enum
    {
        var normalised = Regex.Replace(value, @"[^a-zA-Z0-9]", "");

        var match = Enum.GetNames<T>()
            .FirstOrDefault(name =>
                string.Equals(
                    Regex.Replace(name, @"[^a-zA-Z0-9]", ""),
                    normalised,
                    StringComparison.OrdinalIgnoreCase
                ));

        if (match is null)
        {
            throw new ArgumentException($"'{value}' could not be mapped to {typeof(T).Name}");
        }

        return Enum.Parse<T>(match);
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
    public static bool TryParseFlexible<T>(string? value, out T result) where T : struct, Enum
    {
        if (string.IsNullOrEmpty(value))
        {
            result = default;
            return false;
        }
        
        try
        {
            result = ParseFlexible<T>(value);
            return true;
        }
        catch
        {
            result = default; 
            return false;
        }
    }
}