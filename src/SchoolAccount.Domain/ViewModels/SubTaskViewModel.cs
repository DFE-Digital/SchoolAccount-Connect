using System.Globalization;
using SchoolAccount.Domain.Dtos;
using SchoolAccount.Domain.Workflow;
using SchoolAccount.Kernel;

namespace SchoolAccount.Domain.ViewModels
{
    public class SubTaskViewModel
    {
        public string Name { get; }
        public string Description { get; }
        public string? DigitalLink { get; }
        public int? RequirementId { get; }
        public bool HasLinks => !string.IsNullOrWhiteSpace(DigitalLink);
        public bool HasDescription => !string.IsNullOrWhiteSpace(Description);
        public bool IsOptional =>
            RequirementId.HasValue ? RequirementId.Value == (int)RequirementValues.Optional : false;
        public string? AvailabilityLabel { get; private set; }
        public DateOnly? SortingDate { get; }
        public string? DueDateLabel { get; }
        private bool? StartDateIsExact { get; }
        private bool? DueDateIsExact { get; }
        private DateOnly? DueDate { get; }
        private DateOnly? StartDate { get; }
        private WorkflowState WorkflowState { get; }
        private bool HasExactStartDate { get; }
        private bool DoesntHaveExactStartDate { get; }
        private bool DoesntHaveStartButHasDueDate { get; }
        private IDateTimeProvider _dateTimeProvider;
        private const string AvailableNow = "Available Now";

        public SubTaskViewModel(SubTaskListItemDto subTask, IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            Name = subTask.Name;
            Description = subTask.Description;
            DueDate = subTask.DueDate;
            StartDate = subTask.StartDate;
            StartDateIsExact = subTask.StartDateExact;
            DueDateIsExact = subTask.DueDateIsExact;
            RequirementId = subTask.RequirementId;
            DigitalLink = subTask.DigitalLink;
            DueDateIsExact = subTask.DueDateIsExact;
            WorkflowState = subTask.WorkflowState;
            HasExactStartDate = StartDate.HasValue && StartDateIsExact.HasValue && StartDateIsExact.Value == true;
            DoesntHaveExactStartDate =
                StartDate.HasValue && StartDateIsExact.HasValue && StartDateIsExact.Value == false;
            DoesntHaveStartButHasDueDate = !StartDate.HasValue && !StartDateIsExact.HasValue && DueDate.HasValue;

            SetAvailabilityForPublishedTasks();

            SetAvailabilityForExpiredTasks();

            DueDateLabel = "No due date";
            SortingDate = StartDate;
            if (DueDate.HasValue && DueDateIsExact.HasValue)
            {
                DueDateLabel =
                    $"Due {DueDate.Value.ToString(DueDateIsExact.Value ? "d MMM yyyy" : "MMM yyyy", CultureInfo.CurrentCulture)}.";
                SortingDate = DueDate;
            }
        }

        private void SetAvailabilityForPublishedTasks()
        {
            if (WorkflowState == WorkflowState.Published)
            {
                SetAvailabilityLabelWhenPublishedAndHasExactStartDate();
                SetAvailabilityLabelWhenPublishedAndDoesntHaveStartDate();
                SetAvailabilityLabelWhenPublisheddAndDoesntHaveExactStartDate();
            }
        }

        private void SetAvailabilityForExpiredTasks()
        {
            if (WorkflowState == WorkflowState.Expired)
            {
                SetAvailabilityLabelWhenExpiredAndHasExactStartDate();
                SetAvailabilityLabelWhenExpiredAndDoesntHaveExactStartDate();
                SetAvailabilityLabelWhenExpiredAndDoesntHaveStartDate();
            }
        }

        private void SetAvailabilityLabelWhenExpiredAndHasExactStartDate()
        {
            if (HasExactStartDate)
            {
                AvailabilityLabel = $"Available {StartDate?.ToString("d MMM yyyy", CultureInfo.CurrentCulture)}.";
            }
        }

        private void SetAvailabilityLabelWhenExpiredAndDoesntHaveExactStartDate()
        {
            if (DoesntHaveExactStartDate)
            {
                AvailabilityLabel = $"Available {StartDate?.ToString("MMM yyyy", CultureInfo.CurrentCulture)}.";
            }
        }

        private void SetAvailabilityLabelWhenExpiredAndDoesntHaveStartDate()
        {
            if (DoesntHaveStartButHasDueDate)
            {
                AvailabilityLabel = string.Empty;
            }
        }

        private void SetAvailabilityLabelWhenPublishedAndHasExactStartDate()
        {
            if (HasExactStartDate && StartDate.HasValue)
            {
                AvailabilityLabel =
                    StartDate.Value > DateOnly.FromDateTime(_dateTimeProvider.UtcNow)
                        ? $"Available {StartDate?.ToString("d MMM yyyy", CultureInfo.CurrentCulture)}."
                        : AvailabilityLabel = $"{AvailableNow}.";
            }
        }

        private void SetAvailabilityLabelWhenPublisheddAndDoesntHaveExactStartDate()
        {
            if (DoesntHaveExactStartDate)
            {
                DateTime? nextMonth = StartDate.HasValue
                    ? StartDate.Value.AddMonths(1).ToDateTime(TimeOnly.Parse("00:00 AM", CultureInfo.CurrentCulture))
                    : null;
                AvailabilityLabel =
                    _dateTimeProvider.UtcNow < nextMonth
                        ? $"Available {StartDate?.ToString("MMM yyyy", CultureInfo.CurrentCulture)}."
                        : $"{AvailableNow}.";
            }
        }

        private void SetAvailabilityLabelWhenPublishedAndDoesntHaveStartDate()
        {
            if (DoesntHaveStartButHasDueDate)
            {
                AvailabilityLabel = $"{AvailableNow}.";
            }
        }
    }
}
