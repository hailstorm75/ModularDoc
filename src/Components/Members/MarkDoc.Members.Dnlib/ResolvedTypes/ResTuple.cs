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
  /// Class for representing resolved tuples
  /// </summary>
  [DebuggerDisplay(nameof(ResTuple) + ": {" + nameof(DisplayName) + "}")]
  public class ResTuple
    : ResType, IResTuple
  {
    #region Properties

    /// <inheritdoc />
    public IReadOnlyCollection<(string, IResType)> Fields { get; }

    /// <inheritdoc />
    public bool IsValueTuple { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="isValueTuple">Is the given tuple a value tuple</param>
    /// <param name="isByRef"></param>
    internal ResTuple(IResolver resolver, TypeSig source, bool isValueTuple, bool isByRef = false)
      : base(resolver, source, ResolveName(resolver, source, isValueTuple, out var fields), ResolveDocName(source), source.FullName, isByRef)
    {
      IsValueTuple = isValueTuple;
      Fields = fields;
    }

    private static string ResolveName(IResolver resolver, TypeSig source, bool isValueTuple, out IReadOnlyCollection<(string, IResType)> fields)
    {
      // If the source is not a generic instance..
      if (!(source is GenericInstSig token))
        // throw an exception
        throw new NotSupportedException(Resources.notTuple);

      fields = token.GenericArguments.Select((x, i) => ($"Item{i + 1}", resolver.Resolve(x))).ToReadOnlyCollection();
      return isValueTuple
        ? $"({string.Join(", ", fields.Select(field => $"{field.Item2.DisplayName} {field.Item1}"))})"
        : $"Tuple<{string.Join(",", fields.Select(field => field.Item2.DisplayName))}>";
    }
  }
}
