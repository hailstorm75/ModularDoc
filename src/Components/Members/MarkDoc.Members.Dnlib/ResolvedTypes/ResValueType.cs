using System;
using System.Diagnostics;
using dnlib.DotNet;

namespace MarkDoc.Members.Dnlib.ResolvedTypes
{
  /// <summary>
  /// Class representing resolved value types
  /// </summary>
  [DebuggerDisplay(nameof(ResValueType) + ": {" + nameof(DisplayName) + "}")]
  public class ResValueType
    : ResType
  {
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="displayName">Type display name</param>
    /// <param name="isByRef">Indicates whether the type is by references</param>
    internal ResValueType(Resolver resolver, TypeSig source, string displayName, bool isByRef = false)
      : this(resolver, source, displayName, source.FullName, isByRef) { }

    /// <summary>
    /// Inherited constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="displayName">Type displayed name</param>
    /// <param name="docName">Type documentation name</param>
    /// <param name="isByRef">Indicates whether the type is by references</param>
    protected ResValueType(Resolver resolver, TypeSig source, string displayName, string docName, bool isByRef = false)
      // ReSharper disable once ConstantConditionalAccessQualifier
      : base(resolver, source, displayName, docName, source?.FullName ?? throw new ArgumentNullException(nameof(source)), isByRef) { }
  }
}
