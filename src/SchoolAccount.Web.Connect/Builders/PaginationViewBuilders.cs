using Microsoft.AspNetCore.WebUtilities;
using SchoolAccount.Web.Connect.Builders.Interfaces;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Models;
using SchoolAccount.Web.Connect.Models.Interfaces;
using X.PagedList;

namespace SchoolAccount.Web.Connect.Builders;

public class PaginationViewBuilder(IHttpContextAccessor contextAccessor) : IPaginationViewBuilder
{
    private const int EllipsisIdentifier = -1;

    private static string? BuildUrl(string endpoint, int pageNumber, HttpRequest request)
    {
        ArgumentNullException.ThrowIfNull(endpoint);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageNumber, 0);

        var query = request
            .Query.SelectMany(q => q.Value.Select(v => new KeyValuePair<string, string?>(q.Key, v)))
            .ToList();

        for (var i = query.Count - 1; i >= 0; i--)
        {
            var pair = query[i];

            if (pair.Key.Equals("pageNumber", StringComparison.OrdinalIgnoreCase))
            {
                query.RemoveAt(i);
            }
        }

        query.Add(
            new KeyValuePair<string, string?>("pageNumber", pageNumber.ToString(Thread.CurrentThread.CurrentCulture))
        );

        return QueryHelpers.AddQueryString($"/{endpoint.TrimStart('/')}", query);
    }

    private static List<int> GetListOfPagesToShow(Pagination model)
    {
        var pagesToShow = new List<int>();

        if (model.TotalPages <= 7)
        {
            pagesToShow.AddRange(Enumerable.Range(1, model.TotalPages));
        }
        else
        {
            pagesToShow.Add(1);

            var left = Math.Max(2, model.PageNumber - 1);
            var right = Math.Min(model.TotalPages - 1, model.PageNumber + 1);

            if (left > 2)
            {
                pagesToShow.Add(EllipsisIdentifier);
            }

            for (var p = left; p <= right; p++)
                pagesToShow.Add(p);

            if (right < model.TotalPages - 1)
            {
                pagesToShow.Add(EllipsisIdentifier);
            }

            pagesToShow.Add(model.TotalPages);
        }

        return pagesToShow;
    }

    public PaginationViewModel Build(Pagination model, string endpoint, HttpRequest? request = null)
    {
        request ??= contextAccessor.HttpContext?.Request ?? throw new ArgumentNullException(nameof(request));

        var items = GetListOfPagesToShow(model)
            .Select<int, IPaginationItem>(p =>
            {
                if (p == EllipsisIdentifier)
                {
                    return new PaginationEllipsisViewModel();
                }

                return new PaginationItemViewModel(p, BuildUrl(endpoint, p, request)!, p == model.PageNumber);
            })
            .ToList();

        return new PaginationViewModel(items)
        {
            PreviousUrl = model.HasPreviousPage ? BuildUrl(endpoint, model.PageNumber - 1, request) : null,
            NextUrl = model.HasNextPage ? BuildUrl(endpoint, model.PageNumber + 1, request) : null,
            PageCount = model.PageCount,
            TotalItemCount = model.TotalItemCount,
            FirstItemOnPage = model.FirstItemOnPage,
            LastItemOnPage = model.LastItemOnPage,
            PageNumber = model.PageNumber,
            PageSize = model.PageSize,
            IsFirstPage = model.IsFirstPage,
            IsLastPage = model.IsLastPage,
            HasNextPage = model.HasNextPage,
            HasPreviousPage = model.HasPreviousPage,
        };
    }

    public PaginationViewModel Build<T>(T model)
        where T : IPagedList
    {
        return Build(
            new Pagination(model),
            contextAccessor.HttpContext!.GetCurrentEndpoint(),
            contextAccessor.HttpContext!.Request
        );
    }
}
