using System;
using System.Diagnostics;

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
    /// <param name="displayName"></param>
    internal ResValueType(IResolver resolver, dnlib.DotNet.TypeSig source, string displayName)
      : this(resolver, source, displayName, source.FullName) { }

    /// <summary>
    /// Inherited constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="displayName">Type displayed name</param>
    /// <param name="docName">Type documentation name</param>
    protected ResValueType(IResolver resolver, dnlib.DotNet.TypeSig source, string displayName, string docName)
      // ReSharper disable once ConstantConditionalAccessQualifier
      : base(resolver, source, displayName, docName, source?.FullName ?? throw new ArgumentNullException(nameof(source))) { }
  }
}
