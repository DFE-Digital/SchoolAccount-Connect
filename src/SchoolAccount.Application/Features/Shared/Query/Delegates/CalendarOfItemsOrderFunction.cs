using SchoolAccount.Application.Features.Shared.Query.Interfaces;

namespace SchoolAccount.Application.Features.Shared.Query.Delegates;

public delegate IOrderedQueryable<T> GenericOrderFunction<T>(
    IQueryable<T> query
) where T : IQueryRow;