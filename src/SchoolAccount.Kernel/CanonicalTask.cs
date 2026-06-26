using System.Diagnostics.CodeAnalysis;

namespace SchoolAccount.Kernel;

/// <summary>
/// The single common representation every integration maps into. Strongly-typed
/// canonical fields cover the common case; integration-specific extras live in
/// <see cref="Metafields"/>. Field access is also available by name (so governance
/// policies and Scriban transforms can address fields generically).
/// </summary>
public sealed record CanonicalTask
{
    public required string Id { get; init; }

    /// <summary>Integration key the task came from (e.g. "jira").</summary>
    public required string Source { get; init; }

    public required string Title { get; init; }
    public string? Description { get; init; }
    public string State { get; init; } = "Unknown";
    public DateTimeOffset? DueDate { get; init; }
    public string? AssigneeEmail { get; init; }
    public IReadOnlyList<string> Labels { get; init; } = Array.Empty<string>();
    public string? Url { get; init; }

    /// <summary>Integration-specific fields that don't map onto a canonical property.</summary>
    public IReadOnlyDictionary<string, object?> Metafields { get; init; }
        = new Dictionary<string, object?>();

    /// <summary>Read a field by name (canonical property or extension), case/underscore-insensitive.</summary>
    public object? GetField(string field) => Normalize(field) switch
    {
        "id" => Id,
        "source" => Source,
        "title" => Title,
        "description" => Description,
        "state" => State,
        "duedate" => DueDate,
        "assigneeemail" => AssigneeEmail,
        "labels" => Labels,
        "url" => Url,
        _ => Metafields.TryGetValue(field, out var v) ? v : null,
    };

    /// <summary>Return a copy with a single field replaced (canonical property or extension).</summary>
    public CanonicalTask WithField(string field, object? value) => Normalize(field) switch
    {
        "title" => this with { Title = value?.ToString() ?? string.Empty },
        "description" => this with { Description = value?.ToString() },
        "assigneeemail" => this with { AssigneeEmail = value?.ToString() },
        "url" => this with { Url = value?.ToString() },
        "duedate" => this with { DueDate = AsDate(value) },
        _ => this with
        {
            Metafields = new Dictionary<string, object?>(Metafields) { [field] = value },
        },
    };

    [SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase")]
    private static string Normalize(string field) => field
        .Replace("_", "", StringComparison.InvariantCultureIgnoreCase)
        .ToLowerInvariant();

    private static DateTimeOffset? AsDate(object? value) => value switch
    {
        null => null,
        DateTimeOffset dto => dto,
        DateTime dt => dt,
        string s when DateTimeOffset.TryParse(s, out var d) => d,
        _ => null,
    };
}
