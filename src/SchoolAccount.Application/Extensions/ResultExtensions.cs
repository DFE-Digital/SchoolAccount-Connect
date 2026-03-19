using FluentValidation.Results;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Extensions;

public static class ResultExtensions
{
    public static Result ToResult(this ValidationResult result)
    {
        if (result.IsValid)
        {
            return Result.Success();
        }

        var error = result.Errors.First();
        return Result.Failure(Error.Problem(error.ErrorCode, error.ErrorMessage));
    }

    public static Result<T> ToResult<T>(this ValidationResult result)
        where T : class
    {
        var converted = result.ToResult();

        return converted.IsSuccess ? Result.Success<T>() : Result.Failure<T>(converted.Error);
    }
}
