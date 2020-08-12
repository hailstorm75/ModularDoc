using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using MarkDoc.Helpers;
using MarkDoc.Members.Dnlib.Properties;
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
    internal ResGeneric(IResolver resolver, TypeSig source, IReadOnlyDictionary<string, string>? generics)
      : base(resolver, source, ResolveName(source), ResolveRawName(resolver, source, generics), source.FullName)
    {
      // If the source is not generic..
      if (!(source is GenericInstSig token))
        // throw an exception
        throw new NotSupportedException(Resources.notGeneric);

      Generics = token.GenericArguments.Select(x => Resolver.Resolve(x, generics)).ToReadOnlyCollection();
    }

    private static string ResolveRawName(IResolver resolver, TypeSig source, IReadOnlyDictionary<string, string>? generics)
    {
      static string ResolveGenerics(string type, IReadOnlyDictionary<string, string>? generics)
      {
        // If the list of known generics and the given type is known..
        if (generics != null && generics.TryGetValue(type, out var generic))
          // return the retrieved result
          return generic;
        // Otherwise return as is
        return type;
      }

      // If the source is not generic..
      if (!(source is GenericInstSig token))
        // throw an exception
        throw new NotSupportedException(Resources.notGeneric);

      // Find type generics
      var index = source.FullName.IndexOf('`', StringComparison.InvariantCultureIgnoreCase);
      // Remove the generics
      var name = source.FullName.Remove(index);

      // Return the reformatted documentation name
      return $"{name}{{{string.Join(",",token.GenericArguments.Select(x => ResolveGenerics(resolver.Resolve(x, generics).DocumentationName, generics)))}}}";
    }
  }
}
