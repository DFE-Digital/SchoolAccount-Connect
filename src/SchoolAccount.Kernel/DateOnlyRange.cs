namespace SchoolAccount.Kernel;

public record struct DateOnlyRange(DateOnly Start, DateOnly End)
{
    public static implicit operator DateOnlyRange(DateOnlyNullableRange range)
    {
        return new DateOnlyRange(range.Start ?? DateOnly.MinValue, range.End ?? DateOnly.MaxValue);
    }
}

public record struct DateOnlyNullableRange(DateOnly? Start, DateOnly? End)
{
    public static implicit operator DateOnlyNullableRange(DateOnlyRange range)
    {
        return new DateOnlyNullableRange(range.Start, range.End);
    }
}
