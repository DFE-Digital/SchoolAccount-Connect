using Xunit;

namespace SchoolAccount.Domain.UnitTests.Subtasks;

public class AvailabilityBoundaryData : TheoryData<string, string, bool, string, string>
{
    public AvailabilityBoundaryData()
    {
        // --- Summer Time (BST) Boundaries ---
        // In June, 11:00 PM UTC = Midnight (00:00) in the UK.

        Add(
            "2026-06-14T23:00:00Z",
            "2026-06-15",
            true,
            "Available Now.",
            "at 11pm UTC in June, the UK has already hit midnight, so the task is available"
        );

        Add(
            "2026-06-14T22:59:59Z",
            "2026-06-15",
            true,
            "Available 15 Jun 2026.",
            "one second before 11pm UTC in June, it is still 11:59pm on the 14th in the UK"
        );

        // --- Winter Time (GMT) Boundaries ---
        // In January, 11:00 PM UTC = 11:00 PM in the UK.

        Add(
            "2026-01-14T23:00:00Z",
            "2026-01-15",
            true,
            "Available 15 Jan 2026.",
            "at 11pm UTC in January, it is still the 14th in the UK (GMT)"
        );

        Add(
            "2026-01-15T00:00:00Z",
            "2026-01-15",
            true,
            "Available Now.",
            "at exactly midnight UTC in January, the UK date rolls over to the 15th"
        );

        // --- Month Rollovers (Approximate Dates) ---
        // Approximate dates become available when the NEXT month begins in the UK.

        Add(
            "2026-06-30T23:00:00Z",
            "2026-06-01",
            false,
            "Available Now.",
            "at 11pm UTC on the last day of June, July has begun in the UK"
        );

        Add(
            "2026-06-30T22:59:59Z",
            "2026-06-01",
            false,
            "Available Jun 2026.",
            "one second before July begins in the UK, the label still shows the current month"
        );

        // --- BST Transition Days (Spring Forward / Fall Back) ---

        Add(
            "2026-03-30T01:00:00Z",
            "2026-03-30",
            true,
            "Available Now.",
            "handling the day after the clocks spring forward"
        );

        Add("2026-10-25T01:00:00Z", "2026-10-25", true, "Available Now.", "handling the day the clocks fall back");
    }
}
