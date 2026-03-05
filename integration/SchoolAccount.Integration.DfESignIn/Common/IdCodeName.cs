namespace SchoolAccount.Integration.DfESignIn.Common;

public class IdCodeName<TId, TCode> : IdName<TId>
{
    public TCode? Code { get; init; }
}