using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using ModularDoc.Helpers;
using ModularDoc.Members.Dnlib.Helpers;
using ModularDoc.Members.ResolvedTypes;

namespace ModularDoc.Members.Dnlib.ResolvedTypes
{
  /// <summary>
  /// Class for representing resolved generic types
  /// </summary>
  [DebuggerDisplay(nameof(ResGeneric) + ": {" + nameof(DisplayName) + "}")]
  public class ResGeneric
    : ResType, IResGeneric
  {
    #region Properties

    /// <inheritdoc />
    public IReadOnlyCollection<IResType> Generics { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="generics">List of known generics</param>
    /// <param name="dynamicsMap">Map indicating what types are dynamic</param>
    /// <param name="tupleMap">Map indicating value tuple names</param>
    /// <param name="isByRef">Indicates whether the type is by references</param>
    internal ResGeneric(Resolver resolver, TypeSig source, IReadOnlyDictionary<string, string>? generics, IEnumerable<bool>? dynamicsMap, IReadOnlyList<string>? tupleMap, bool isByRef = false)
      : base(resolver, source, ResolveName(source), ResolveRawName(resolver, source, dynamicsMap, tupleMap, generics, out var genericsProcessed), source.FullName, isByRef)
      => Generics = genericsProcessed;

    private static string ResolveRawName(Resolver resolver, TypeSig source, IEnumerable<bool>? dynamicsMap, IReadOnlyList<string>? tupleMap, IReadOnlyDictionary<string, string>? generics, out IReadOnlyCollection<IResType> genericsProcessed)
    {
      static string ResolveGenerics(string type, IReadOnlyDictionary<string, string>? generics)
      {
        // If the list of known generics and the given type is known..
        if (generics != null
            && generics.TryGetValue(type, out var generic))
          // return the retrieved result
          // ReSharper disable once AssignNullToNotNullAttribute
          return generic;
        // Otherwise return as is
        return type;
      }

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

      // Cast the source to a generic type
      var genericType = source.GetGenericSignature();

      // Find type generics
      var index = source.FullName.IndexOf('`', StringComparison.InvariantCultureIgnoreCase);
      // Remove the generics
      var name = source.FullName.Remove(index).Replace('/', '.');

      // Count the number of types in each generic arguments branch
      var parametersTree = genericType.CountTypes();

      genericsProcessed = genericType.GenericArguments
        // Process the generic arguments
        .Select((x, i) => resolver.Resolve(x, generics, false, GetGenerics(i, dynamicsMap, parametersTree), tupleMap))
        // Materialize the collection
        .ToReadOnlyCollection();

      // Return the reformatted documentation name
      return $"{name}{{{string.Join(",", genericsProcessed.Select((x, _) => ResolveGenerics(x.DocumentationName, generics)))}}}";
    }
  }
}