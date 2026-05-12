namespace SchoolAccount.Web.Connect.Models.Shared;

public class FieldMetadataModel
{
    public string FieldName { get; init; } = null!;
    public string FieldId { get; init; } = null!;

    public string Label { get; init; } = null!;

    public string? Description { get; init; }
    public bool HasDescription => !string.IsNullOrEmpty(Description);

    public string? Hint { get; init; }
    public bool HasHint => !string.IsNullOrEmpty(Hint);

    public bool IsRequired { get; init; }

    public bool HasError { get; init; }
    public string? ErrorMessage { get; init; }
    public bool HasValidError => HasError && !string.IsNullOrEmpty(ErrorMessage);
}
