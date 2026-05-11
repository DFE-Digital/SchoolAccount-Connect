using SchoolAccount.Domain.Common;
using Xunit;
using static SchoolAccount.Domain.Common.WorkflowState;

namespace SchoolAccount.Domain.UnitTests.Subtasks;

public class AvailabilityScenarioData : TheoryData<WorkflowState, int?, bool?, int?, string, string>
{
    public AvailabilityScenarioData()
    {
        // --- Published State: Exact Dates ---
        Add(
            Published,
            5,
            true,
            null,
            "Available 15 Apr 2026.",
            "a future exact start date should show the specific day"
        );

        Add(Published, -5, true, null, "Available Now.", "a past exact start date is already active");

        Add(Published, 0, true, null, "Available Now.", "a task starting today is available immediately");

        // --- Published State: Approximate Dates ---
        Add(
            Published,
            0,
            false,
            null,
            "Available Apr 2026.",
            "for approximate dates, the task isn't 'Available Now' until the following month begins"
        );

        Add(
            Published,
            -40,
            false,
            null,
            "Available Now.",
            "the approximate start month (March) has ended and we are now in April"
        );

        // --- Published State: Edge Cases ---
        Add(
            Published,
            null,
            null,
            30,
            "Available Now.",
            "published tasks with no start date but a future due date are available immediately"
        );

        // --- Expired State Logic ---
        Add(
            Expired,
            -116,
            true,
            null,
            "Available 15 Dec 2025.",
            "expired tasks show the historical exact date for audit purposes"
        );

        Add(
            Expired,
            -160,
            false,
            null,
            "Available Nov 2025.",
            "expired tasks show the historical approximate month for audit purposes"
        );

        // --- Negative / State Constraints ---
        Add(Published, 0, null, null, string.Empty, "no label should be shown if the 'exactness' flag is missing");

        Add(Draft, 0, true, null, string.Empty, "draft tasks should never display availability information");

        Add(Archived, 0, true, null, string.Empty, "archived tasks should never display availability information");
    }
}
