namespace Synith.Core.Extensions;
public static class IEnumerableExtension
{
    public static IEnumerable<T2> Map<T1, T2>(this IEnumerable<T1> items, Func<T1, T2> mapFunction)
        where T1 : class
        where T2 : class
    {
        foreach (var item in items)
        {
            yield return mapFunction.Invoke(item);
        }
    }
}
