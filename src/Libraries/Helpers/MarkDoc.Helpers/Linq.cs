using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkDoc.Helpers
{
  public static class Linq
  {
    public static T XtoX<T>(T input)
      => input;

    public static TKey GroupKey<TKey, TValue>(IGrouping<TKey, TValue> grouping)
    {
      if (grouping == null)
        throw new ArgumentNullException(nameof(grouping));
      return grouping.Key;
    }

    public static IEnumerable<TValue> GroupValues<TKey, TValue>(this IGrouping<TKey, TValue> grouping)
    {
      if (grouping == null)
        throw new ArgumentNullException(nameof(grouping));
      return grouping.Select(XtoX);
    }

    public static IEnumerable<TValue> GroupValuesOfValues<TKey, TValue>(this IGrouping<TKey, IEnumerable<TValue>> grouping)
    {
      if (grouping == null)
        throw new ArgumentNullException(nameof(grouping));
      return grouping.SelectMany(XtoX);
    }

    public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> input)
      => new LinkedList<T>(input);

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> input)
      where T : class
      => input.Where(x => x != null).Cast<T>();
  }
}
