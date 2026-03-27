using System.Collections.ObjectModel;

namespace SchoolAccount.Web.Connect.Extensions;

public static class CollectionExtensions
{
    public static Collection<T> ToCollection<T>(this List<T> collection)
    {
        return new Collection<T>(collection);
    }

    public static Collection<T> ToCollection<T>(this IEnumerable<T> collection)
    {
        return new Collection<T>(collection.ToList());
    }
}
