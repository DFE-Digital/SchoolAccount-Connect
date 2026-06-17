using SchoolAccount.Application.Common;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Models.Interfaces;
using SchoolAccount.Web.Connect.Models.Shared;
using X.PagedList;

namespace SchoolAccount.Web.Connect.Builders;

public class PaginationViewBuilder
{
    private const int EllipsisIdentifier = -1;

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

    private static PaginationViewModel Build(Pagination model, Uri currentUri)
    {
        var items = GetListOfPagesToShow(model)
            .Select<int, IPaginationItem>(page =>
            {
                if (page == EllipsisIdentifier)
                {
                    return new PaginationEllipsisViewModel();
                }

                var pageUri = currentUri.SetQueryParam("pageNumber", page);
                var isCurrentPage = page == model.PageNumber;

                return new PaginationItemViewModel(page, pageUri, isCurrentPage);
            })
            .ToList();

        return new PaginationViewModel(items)
        {
            PreviousUrl = model.HasPreviousPage ? currentUri.SetQueryParam("pageNumber", model.PageNumber - 1) : null,
            NextUrl = model.HasNextPage ? currentUri.SetQueryParam("pageNumber", model.PageNumber + 1) : null,
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

    public PaginationViewModel Build<T>(T model, Uri currentUri)
        where T : IPagedList
    {
        return Build(new Pagination(model), currentUri);
    }
}
