using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using SchoolAccount.Domain.Dtos;
using SchoolAccount.Domain.Enums;
using SchoolAccount.Kernel;

namespace SchoolAccount.Domain.ViewModels
{
    public class TaskDetailsViewModel
    {
        public long Id { get; }
        public string? TaskReferenceNo { get; }

        public string TaskName { get; } = null!;

        public string TaskNameLowerCase => TaskName.ToLower(CultureInfo.CurrentCulture) ?? string.Empty;

        public string? UpdatedBy { get; }

        public DateTime? DateUpdated { get; }

        public TabViewModel? TabViewModel { get; private set; }

        public string? LastUpdatedDate { get; private set; }

        private string? LastTaskUpdated =>
            DateUpdated.HasValue
                ? DateOnly.FromDateTime(DateUpdated.Value).ToString("d MMMM yyyy", CultureInfo.CurrentCulture)
                : null;

        private readonly IDateTimeProvider _dateTimeProvider;

        public TaskDetailsViewModel(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public TaskDetailsViewModel(
            TaskListItemWithSubTaskList taskWithSubTasks,
            TaskDetailViewMode tabIndex,
            IDateTimeProvider dateTimeProvider
        )
        {
            _dateTimeProvider = dateTimeProvider;

            Id = taskWithSubTasks.Task.Id;
            TaskReferenceNo = taskWithSubTasks.Task.ReferenceNo;
            TaskName = taskWithSubTasks.Task.Name;
            UpdatedBy = taskWithSubTasks.Task.UpdatedBy;
            DateUpdated = taskWithSubTasks.Task.DateUpdated;

            SetupTasks(taskWithSubTasks.SubTasks, tabIndex);
        }

        public void AddRequestDetails(HttpRequest request)
        {
            if (TabViewModel != null)
            {
                var url = new Uri(request.GetDisplayUrl());
                TabViewModel.Path = $"{url.Scheme}://{url.Authority}{url.LocalPath}";
            }
        }

        private void SetupTasks(IReadOnlyCollection<SubTaskListItemDto> subTasks, TaskDetailViewMode tabIndex)
        {
            switch (tabIndex)
            {
                case TaskDetailViewMode.PreviousTasks:
                    LastUpdatedDate = SetLastUpdatedDate(subTasks);
                    TabViewModel = new TabViewModel(tabIndex, AddPreviousTasksWhenExpired(subTasks), Id);
                    break;
                case TaskDetailViewMode.UpcomingTasks:
                    LastUpdatedDate = LastTaskUpdated;
                    TabViewModel = new TabViewModel(tabIndex, AddUpcomingTasksWhenPublished(subTasks), Id);
                    break;
                default:
                    LastUpdatedDate = SetLastUpdatedDate(subTasks);
                    TabViewModel = new TabViewModel(tabIndex, AddPreviousTasksWhenExpired(subTasks), Id);
                    break;
            }
        }

        private string? SetLastUpdatedDate(IReadOnlyCollection<SubTaskListItemDto> subTasks)
        {
            DateOnly lastUpdated;
            var subtask = subTasks
                ?.Where(x => x.WorkflowState == WorkflowState.Expired)
                .OrderByDescending(x => x.DateUpdated)
                .FirstOrDefault();
            if (subtask != null)
            {
                lastUpdated = DateOnly.FromDateTime(subtask.DateUpdated);
                return lastUpdated.ToString("d MMMM yyyy", CultureInfo.CurrentCulture);
            }
            else
            {
                return LastTaskUpdated;
            }
        }

        private List<SubTaskViewModel>? AddPreviousTasksWhenExpired(IReadOnlyCollection<SubTaskListItemDto> subTasks)
        {
            var previousSubTasks = new List<SubTaskViewModel>();
            foreach (var subTask in subTasks)
            {
                if (subTask.IsExpiredAndHasStartAndEndDate)
                {
                    previousSubTasks.Add(new SubTaskViewModel(subTask, _dateTimeProvider));
                }
            }

            return previousSubTasks.OrderByDescending(x => x.SortingDate).ToList() ?? [];
        }

        private List<SubTaskViewModel>? AddUpcomingTasksWhenPublished(IReadOnlyCollection<SubTaskListItemDto> subTasks)
        {
            var upcomingSubTasks = new List<SubTaskViewModel>();
            foreach (var subTask in subTasks)
            {
                if (subTask.IsPublishedAndHasStartAndEndDate)
                {
                    upcomingSubTasks.Add(new SubTaskViewModel(subTask, _dateTimeProvider));
                }
            }

            return upcomingSubTasks.OrderByDescending(x => x.SortingDate).ToList() ?? [];
        }
    }
}
