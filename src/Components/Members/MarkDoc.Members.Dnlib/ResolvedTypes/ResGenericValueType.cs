using System;
using System.Collections.Generic;
using System.Diagnostics;
using dnlib.DotNet;

namespace MarkDoc.Members.Dnlib.ResolvedTypes
{
  /// <summary>
  /// Class for representing resolved generic value types
  /// </summary>
  [DebuggerDisplay(nameof(ResGenericValueType) + ": {" + nameof(DisplayName) + "}")]
  public class ResGenericValueType
    : ResValueType
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="generics">List of known generics</param>
    /// <param name="isByRef"></param>
    internal ResGenericValueType(Resolver resolver, TypeSig source, IReadOnlyDictionary<string, string>? generics, bool isByRef = false)
      : base(resolver, source ?? throw new ArgumentNullException(nameof(source)), source.FullName, ResolveName(source, generics), isByRef) { }

    private static string ResolveName(IFullName source, IReadOnlyDictionary<string, string>? generics)
    {
      // If the list of generics is not null and the source is known..
      if (generics != null && generics.TryGetValue(source.FullName, out var result))
        // return the found result
        return result!;

      // Otherwise return the full name
      return source.FullName;
    }
  }
}
