namespace SchoolAccount.Kernel;

public class MinMax<T>(T min, T max) where T : struct
{
    public T Min { get; } = min;
    public T Max { get; } = max;
    
    public void Deconstruct(out T start, out T end)
    {
        start = Min;
        end = Max;
    }
}

public class NumericRange(int min, int max) : MinMax<int>(min, max);

public class DateOnlyRange(DateOnly start, DateOnly end) : MinMax<DateOnly>(start, end);

public class DateRange(DateTime start, DateTime end) : MinMax<DateTime>(start, end);