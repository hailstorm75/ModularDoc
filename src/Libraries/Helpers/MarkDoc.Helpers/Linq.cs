using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MarkDoc.Helpers
{
#pragma warning disable CA1724
  public static class Linq
  {
    public static void AddRange<T>(this ISet<T> set, IEnumerable<T> items)
    {
      foreach (var item in items)
        set.Add(item!);
    }

    public static void AddRange<T>(this Collection<T> collection, IEnumerable<T> items)
    {
      foreach (var item in items)
        collection.Add(item!);
    }

    public static void AddSorted<T>(this IList<T> list, T item, IComparer<T>? comparer = default)
    {
      if (comparer == null)
        comparer = Comparer<T>.Default;

      var i = 0;
      while (i < list.Count && comparer.Compare(list[i], item) < 0)
        ++i;

      list.Insert(i, item);
    }

    /// <summary>
    /// Passes the element forward
    /// </summary>
    /// <param name="input">Input to process</param>
    /// <typeparam name="T">Input collection items type</typeparam>
    /// <returns>Item</returns>
    public static T XtoX<T>(T input)
      => input;

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
    public static void AddRange<T>(this LinkedList<T> subject, IEnumerable<T>? input)
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
    /// <param name="predicate">Extracts data from the <paramref name="input"/> collection</param>
    /// <typeparam name="T">Input collection items type</typeparam>
    /// <typeparam name="TProp">Type of the extracted data from the <paramref name="input"/> collection</typeparam>
    /// <returns>Filtered enumeration</returns>
    public static IEnumerable<T> WhereNotNull<T, TProp>(this IEnumerable<T> input, Func<T, TProp?> predicate)
      where TProp : class
      => input.Where(x => predicate(x) != null);

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
