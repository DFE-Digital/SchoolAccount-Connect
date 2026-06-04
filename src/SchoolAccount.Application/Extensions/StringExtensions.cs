using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SchoolAccount.Application.Extensions;

public static class StringExtensions
{
    public static string? ToSentence(this string? text, bool preserveAcronyms)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        var newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        
        for (var i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]))
                if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                    (preserveAcronyms && char.IsUpper(text[i - 1]) && 
                     i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                    newText.Append(' ');
            newText.Append(text[i]);
        }
        
        return newText.ToString();
    }
    
    public static string Format(this string template, object values)
    {
        return Regex.Replace(template, @"\{(\w+)\}", m =>
        {
            var prop = values.GetType().GetProperty(m.Groups[1].Value);
            return prop?.GetValue(values)?.ToString() ?? m.Value;
        });
    }

    public static string Format(this string template, params object[] values)
    {
        return string.Format(CultureInfo.InvariantCulture, template, values);
    }

    public static string RemoveOptionalUrlProperties(this string template)
    {
        return Regex.Replace(template, @"\{.*?\}/?", "").TrimEnd('/');
    }
}