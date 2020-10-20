using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using MarkDoc.Helpers;
using MarkDoc.Members.Dnlib.Helpers;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Dnlib.ResolvedTypes
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
    /// <param name="isDynamic"></param>
    /// <param name="isByRef"></param>
    internal ResGeneric(Resolver resolver, TypeSig source, IReadOnlyDictionary<string, string>? generics,
      IReadOnlyList<bool>? isDynamic, bool isByRef = false)
      : base(resolver, source, ResolveName(source),
        ResolveRawName(resolver, source, isDynamic, generics, out var genericsProcessed), source.FullName, isByRef)
      => Generics = genericsProcessed;

    private static string ResolveRawName(Resolver resolver, TypeSig source, IReadOnlyList<bool>? isDynamic, IReadOnlyDictionary<string, string>? generics, out IReadOnlyCollection<IResType> genericsProcessed)
    {
      static string ResolveGenerics(string type, IReadOnlyDictionary<string, string>? generics)
      {
        // If the list of known generics and the given type is known..
        if (generics != null
            && generics.TryGetValue(type, out var generic))
          // return the retrieved result
          return generic;
        // Otherwise return as is
        return type;
      }

      var token = source.GetGenericSignature();

      // Find type generics
      var index = source.FullName.IndexOf('`', StringComparison.InvariantCultureIgnoreCase);
      // Remove the generics
      var name = source.FullName.Remove(index);

      var parametersTree = token.CountTypes();

      IReadOnlyList<bool> GetGenerics(int i)
      {
        if (isDynamic is null)
          return new bool[parametersTree[i]];

        var skip = i == 0
          ? 0
          : parametersTree.Take(i).Sum();

        var result = isDynamic.Skip(skip).Take(parametersTree[i]).ToArray();
        return result;
      }

      genericsProcessed = token.GenericArguments
        .Select((x, i) => resolver.Resolve(x, generics, isDynamic: GetGenerics(i)))
        .ToReadOnlyCollection();

      // Return the reformatted documentation name
      return
        $"{name}{{{string.Join(",", genericsProcessed.Select((x, i) => ResolveGenerics(x.DocumentationName, generics)))}}}";
    }
  }
}