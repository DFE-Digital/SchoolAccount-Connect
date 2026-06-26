using SchoolAccount.Application.Features.Shared.Query.Interfaces;

namespace SchoolAccount.Application.Features.Shared.Query.Delegates;

public delegate IOrderedEnumerable<T> GenericOrderFunction<T>(IList<T> query)
    where T : IQueryRow;
