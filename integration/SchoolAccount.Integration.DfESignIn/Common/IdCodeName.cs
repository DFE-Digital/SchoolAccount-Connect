namespace SchoolAccount.Integration.DfESignIn.Common;

public record IdCodeName<TId, TCode> : IdName<TId>
{
    public TCode? Code { get; init; }
}
