using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using MarkDoc.Helpers;
using MarkDoc.Members.Dnlib.Helpers;
using MarkDoc.Members.Dnlib.Properties;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Dnlib.ResolvedTypes
{
  /// <summary>
  /// Class for representing resolved tuples
  /// </summary>
  [DebuggerDisplay(nameof(ResTuple) + ": {" + nameof(DisplayName) + "}")]
  public class ResTuple
    : ResType, IResTuple
  {
    #region Properties

    /// <inheritdoc />
    public IReadOnlyCollection<(string name, IResType type)> Fields { get; }

    /// <inheritdoc />
    public bool IsValueTuple { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="isValueTuple">Is the given tuple a value tuple</param>
    /// <param name="generics"></param>
    /// <param name="dynamicsMap">Map indicating what types are dynamic</param>
    /// <param name="tupleMap">Map indicating value tuple names</param>
    /// <param name="isByRef">Indicates whether the type is by references</param>
    internal ResTuple(Resolver resolver,
      TypeSig source,
      bool isValueTuple,
      IReadOnlyDictionary<string, string>? generics,
      IReadOnlyList<bool>? dynamicsMap,
      IReadOnlyList<string>? tupleMap,
      bool isByRef = false)
      : base(resolver, source,
        ResolveName(resolver, source, isValueTuple, generics, dynamicsMap, tupleMap, out var fields),
        ResolveDocName(source), source.FullName, isByRef)
    {
      IsValueTuple = isValueTuple;
      Fields = fields;
    }

    private static string ResolveName(Resolver resolver, TypeSig source, bool isValueTuple,
      IReadOnlyDictionary<string, string>? generics, IReadOnlyList<bool>? dynamicsMap, IReadOnlyList<string>? tupleMap,
      out IReadOnlyCollection<(string, IResType)> fields)
    {
      // If the source is not a generic instance..
      if (!(source is GenericInstSig token))
        // throw an exception
        throw new NotSupportedException(Resources.notTuple);

      // Cast the source to a generic type
      var genericType = source.GetGenericSignature();
      var childrenCount = genericType.CountTypes(true);
      var topNames = tupleMap?.Take(childrenCount.Count).ToArray();
      var children = tupleMap?.Skip(childrenCount.Count).ToArray() ?? Enumerable.Empty<string>();
      var childrenNames = childrenCount
        .Select((x, i) => children
          // Skip the previously selected names
          .Skip(childrenCount.Take(i).Sum())
          // Take the children node names
          .Take(x)
          // Materialize the collection
          .ToArray()
        ).ToArray();

      static IReadOnlyList<bool>? GetGenerics(int i, IEnumerable<bool>? map, IReadOnlyList<int> tree)
      {
        // If there are no dynamic types in the arguments..
        if (map is null)
          // return nothing
          return null;

        // Get the number of previously retrieved indicators from the map
        var skip = i == 0
          // If it is the first retrieval nothing was retrieved previously
          ? 0
          // Otherwise sum the number of previously retrieved indicators
          : tree.Take(i).Sum();

        return map
          // Skip the previously retrieved indicators
          .Skip(skip)
          // Take the required indicators
          .Take(tree[i])
          // Materialize the collection
          .ToArray();
      }

      IReadOnlyList<string>? GetTupleNames(int i)
        => topNames is null || topNames.Length == 0
          ? null
          : childrenNames[i];

      fields = token.GenericArguments
        .Select((x, i) => (isValueTuple && tupleMap != null && tupleMap.Count != 0
            ? tupleMap[i]
            : $"Item{i + 1}",
          resolver.Resolve(x, generics, false, GetGenerics(i, dynamicsMap, genericType.CountTypes()), GetTupleNames(i))))
        .ToReadOnlyCollection();
      return isValueTuple
        ? $"({string.Join(", ", fields.Select(field => $"{field.Item2.DisplayName}{(field.Item1.Length == 0 ? string.Empty : " ")}{field.Item1}"))})"
        : $"Tuple<{string.Join(",", fields.Select(field => field.Item2.DisplayName))}>";
    }
  }
}