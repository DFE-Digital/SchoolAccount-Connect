namespace SchoolAccount.Kernel;

public class Result
{
    public Result()
    {
        Error = null;        
    }
    
    public Result(Error error)
    {
        Error = error;
    }
    
    public bool IsSuccess => Error == null;

    public Error? Error { get; }
    
    public bool IsFailure => !IsSuccess;
}

public class Result<T> : Result
{
    public Result() : base()
    {
    }
    
    public Result(Error error) : base(error)
    {
    }
}