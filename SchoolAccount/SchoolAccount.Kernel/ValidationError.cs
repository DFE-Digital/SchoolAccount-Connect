namespace SchoolAccount.Kernel;

public record ValidationError : Error
{
    public IReadOnlyCollection<Error> Errors { get; }
    public ValidationError(IReadOnlyCollection<Error> errors) : base("Validation.General", "One of more validation errors occured", ErrorType.Validation)
    {
        Errors = errors;
    }
}