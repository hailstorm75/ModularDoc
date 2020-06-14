using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkDoc.Helpers
{
  public static class Linq
  {
    /// <summary>
    /// Passes the element forward
    /// </summary>
    /// <param name="input">Input to process</param>
    /// <typeparam name="T">Input collection items type</typeparam>
    /// <returns>Item</returns>
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
      if (source is null)
        throw new ArgumentNullException(nameof(source));

      var seenKeys = new HashSet<TKey>();
      foreach (var element in source)
        if (seenKeys.Add(keySelector(element)))
          yield return element;
    }

    /// <summary>
    /// Get the key of the <paramrem name="grouping"/>
    /// </summary>
    /// <param name="grouping">Grouping to process</param>
    /// <typeparam name="TKey">Type of the key</typeparam>
    /// <typeparam name="TValue">Type of the value</typeparam>
    /// <returns>Key of the group</returns>
    public static TKey GroupKey<TKey, TValue>(IGrouping<TKey, TValue> grouping)
    {
      if (grouping is null)
        throw new ArgumentNullException(nameof(grouping));
      return grouping.Key;
    }

    /// <summary>
    /// Get the values of the <paramrem name="grouping"/>
    /// </summary>
    /// <param name="grouping">Grouping to process</param>
    /// <typeparam name="TKey">Type of the key</typeparam>
    /// <typeparam name="TValue">Type of the value</typeparam>
    /// <returns>Values of the group</returns>
    public static IEnumerable<TValue> GroupValues<TKey, TValue>(this IGrouping<TKey, TValue> grouping)
    {
      if (grouping is null)
        throw new ArgumentNullException(nameof(grouping));
      return grouping.Select(XtoX);
    }

    /// <summary>
    /// Get the values of the <paramrem name="grouping"/> and flattens them
    /// </summary>
    /// <param name="grouping">Grouping to process</param>
    /// <typeparam name="TKey">Type of the key</typeparam>
    /// <typeparam name="TValue">Type of the value</typeparam>
    /// <returns>Values of the group</returns>
    public static IEnumerable<TValue> GroupValuesOfValues<TKey, TValue>(this IGrouping<TKey, IEnumerable<TValue>> grouping)
    {
      if (grouping is null)
        throw new ArgumentNullException(nameof(grouping));
      return grouping.SelectMany(XtoX);
    }

    /// <summary>
    /// Create a read only collection from the <paramref name="input"/>
    /// </summary>
    /// <param name="input">Input to process</param>
    /// <typeparam name="T">Input collection items type</typeparam>
    /// <returns>Read only collection</returns>
    public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> input)
      => input.ToLinkedList();

    /// <summary>
    /// Create a linked list from the <paramref name="input"/>
    /// </summary>
    /// <param name="input">Input to process</param>
    /// <typeparam name="T">Input collection items type</typeparam>
    /// <returns>Linked list</returns>
    public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> input)
      => new LinkedList<T>(input);

    /// <summary>
    /// Adds a range of nodes to the end of the <see cref="LinkedList{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of the nudes</typeparam>
    /// <param name="subject">List to add items to</param>
    /// <param name="input">Items to add</param>
    public static void AddRange<T>(this LinkedList<T> subject, IEnumerable<T> input)
    {
      if (subject is null)
        throw new ArgumentNullException(nameof(subject));

      foreach (var item in input ?? Enumerable.Empty<T>())
        subject.AddLast(item);
    }

    /// <summary>
    /// Filters out null types
    /// </summary>
    /// <param name="input">Input to process</param>
    /// <typeparam name="T">Input collection items type</typeparam>
    /// <returns>Filtered enumeration</returns>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> input)
      where T : class
      => input.Where(x => x != null)!;
  }
}
