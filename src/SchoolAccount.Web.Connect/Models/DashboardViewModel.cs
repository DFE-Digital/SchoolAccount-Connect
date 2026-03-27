using System.Collections.ObjectModel;
using SchoolAccount.Kernel;

namespace SchoolAccount.Web.Connect.Models;

public class DashboardViewModel(Result outcome, Collection<DashboardViewItem> items)
    : Result<Collection<DashboardViewItem>>(items, outcome.IsSuccess, outcome.Error);
