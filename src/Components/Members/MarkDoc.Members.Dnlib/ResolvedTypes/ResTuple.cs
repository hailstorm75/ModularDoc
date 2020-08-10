using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using MarkDoc.Helpers;
using MarkDoc.Members.Dnlib.Properties;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Dnlib.ResolvedTypes
{
  public class ResTuple
    : ResType, IResTuple
  {
    #region Properties

    public IReadOnlyCollection<(string, IResType)> Fields { get; }

    public bool IsValueTuple { get; }

    #endregion

    internal ResTuple(IResolver resolver, TypeSig source, bool isValueTuple)
      : base(resolver, source, ResolveName(resolver, source, isValueTuple, out var fields), ResolveDocName(source), source.FullName)
    {
      IsValueTuple = isValueTuple;
      Fields = fields;
    }

    private static string ResolveName(IResolver resolver, TypeSig source, bool isValueTuple,
      out IReadOnlyCollection<(string, IResType)> fields)
    {
      if (!(source is GenericInstSig token))
        throw new NotSupportedException(Resources.notTuple);

      fields = token.GenericArguments.Select((x, i) => ($"Item{i + 1}", resolver.Resolve(x))).ToReadOnlyCollection();
      return isValueTuple
        ? $"({string.Join(", ", fields.Select(field => $"{field.Item2.DisplayName} {field.Item1}"))})"
        : $"Tuple<{string.Join(",", fields.Select(field => field.Item2.DisplayName))}>";
    }
  }
}
