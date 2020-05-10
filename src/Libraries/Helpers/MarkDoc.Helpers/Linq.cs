using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkDoc.Helpers
{
  public static class Linq
  {
    public static T XtoX<T>(T input)
      => input;

    /// <summary>
    /// Returns distinct elements from a sequence by using the default equality comparer to compare values by a given key.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey">The type of keys by which the elements from <paramref name="source"/> will be compared by.</typeparam>
    /// <param name="source">A sequence of values to order.</param>
    /// <param name="keySelector">A retrieve function to select each sequence items given key.</param>
    /// <returns>An System.Collections.Generic.IEnumerable`1 that contains distinct elements from the source sequence.</returns>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      var seenKeys = new HashSet<TKey>();
      foreach (var element in source)
        if (seenKeys.Add(keySelector(element)))
          yield return element;
    }


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
