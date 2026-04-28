using System.Diagnostics.CodeAnalysis;
using AwesomeAssertions;
using NSubstitute;
using AwesomeAssertions.Execution;

namespace SchoolAccount.InfrastructureTests.Extensions;

public static class SubstituteAssertionExtensions
{
    public static SubstituteAssertions<T> ShouldHave<T>(this T subject) where T : class
        => new(subject);
}

[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
public class SubstituteAssertions<T>(T subject) where T : class
{
    private readonly T _subject = subject;

    public AndConstraint<SubstituteAssertions<T>> NotReceived(
        Action<T> call,
        string because = "",
        params object[] becauseArgs)
    {
        bool didNotReceive;
        try
        {
            call(_subject.DidNotReceive());
            didNotReceive = true;
        }
        catch
        {
            didNotReceive = false;
        }

        AssertionChain.GetOrCreate()
            .BecauseOf(because, becauseArgs)
            .ForCondition(didNotReceive)
            .FailWith("Expected {0} not to have received the call{reason}, but it did.", typeof(T).Name);

        return new AndConstraint<SubstituteAssertions<T>>(this);
    }

    public AndConstraint<SubstituteAssertions<T>> Received(
        Action<T> call,
        string because = "",
        params object[] becauseArgs)
    {
        bool received;
        try
        {
            call(_subject.Received());
            received = true;
        }
        catch
        {
            received = false;
        }

        AssertionChain.GetOrCreate()
            .BecauseOf(because, becauseArgs)
            .ForCondition(received)
            .FailWith("Expected {0} to have received the call{reason}, but it did not.", typeof(T).Name);

        return new AndConstraint<SubstituteAssertions<T>>(this);
    }
}