using X.PagedList;

namespace SchoolAccount.Web.Connect.Extensions;

public static class PaginationExtensions
{
    public static IPagedList<T> ToStaticPagedList<T>(
        this IEnumerable<T> query,
        int pageNumber,
        int pageSize,
        int totalCount
    )
    {
        return new StaticPagedList<T>(query, pageNumber, pageSize, totalCount);
    }

    public static bool ShouldShowPage(this IPagedList pagedList, int page)
    {
        int current = pagedList.PageNumber;
        int total = pagedList.PageCount;

        // Always show first and last pages
        if (page == 1 || page == total)
            return true;

        // Show current page and its immediate neighbours
        if (Math.Abs(page - current) <= 1)
            return true;

        // GDS rule: If ellipsis would replace a single page (e.g., page 2 or total - 1),
        // show the page number instead.
        if (current <= 4 && page <= 3)
            return true;

        if (current >= total - 3 && page >= total - 2)
            return true;

        return false;
    }

    public static bool ShouldShowEllipsis(this IPagedList pagedList, int page)
    {
        var current = pagedList.PageNumber;
        var total = pagedList.PageCount;

        // Left ellipsis: Only if current page is far enough from the start (>= 5)
        // and we are inspecting the slot right before the visible inner block
        if (current >= 5 && page == 2)
            return true;

        // Right ellipsis: Only if current page is far enough from the end (<= total - 4)
        // and we are inspecting the slot right after the visible inner block
        if (current <= total - 4 && page == total - 1)
            return true;

        return false;
    }

    public static bool IsCurrentPage(this IPagedList pagedList, int page)
    {
        return pagedList.PageNumber == page;
    }
}
