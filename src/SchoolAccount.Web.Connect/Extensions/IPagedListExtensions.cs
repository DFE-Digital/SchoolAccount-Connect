using X.PagedList;

namespace SchoolAccount.Web.Connect.Extensions;

public static class IPagedListExtensions
{
    extension(IPagedList pagedList)
    {
        public bool ShouldShowPage(int page)
        {
            return page == 1 || page == pagedList.PageCount || Math.Abs(page - pagedList.PageNumber) <= 1;
        }

        public bool ShouldShowEllipsis(int page)
        {
            return (page == 2 && pagedList.PageNumber > 4)
                || (page == pagedList.PageCount - 1 && pagedList.PageNumber < pagedList.PageCount - 3);
        }

        public bool IsCurrentPage(int page)
        {
            return pagedList.PageNumber == page;
        }
    }
}
