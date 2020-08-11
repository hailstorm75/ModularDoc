﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using dnlib.DotNet;
using MarkDoc.Members.ResolvedTypes;
using IType = MarkDoc.Members.Types.IType;

namespace MarkDoc.Members.Dnlib.ResolvedTypes
{
  [DebuggerDisplay("{" + nameof(DisplayName) + "}")]
  public class ResType
    : IResType
  {
    #region Properties

    protected IResolver Resolver { get; }

    /// <inheritdoc />
    public string DisplayName { get; }

    /// <inheritdoc />
    public string DocumentationName { get; }

    /// <inheritdoc />
    public string RawName { get; }

    /// <inheritdoc />
    public string TypeNamespace { get; }

    /// <inheritdoc />
    public Lazy<IType?> Reference { get; }

    #endregion

    internal ResType(IResolver resolver, TypeSig source)
      : this(resolver, source, ResolveName(source), ResolveDocName(source), source.FullName) { }

    protected ResType(IResolver resolver, TypeSig source, string displayName, string docName, string rawName)
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));
      if (rawName is null)
        throw new ArgumentNullException(nameof(rawName));

      Resolver = resolver;
      DocumentationName = docName;
      RawName = ProcessRawName(rawName);
      DisplayName = displayName;
      TypeNamespace = source.Namespace;
      Reference = new Lazy<IType?>(() => Resolver.FindReference(source, this), LazyThreadSafetyMode.PublicationOnly);
    }

    #region Methods

    private static string ProcessRawName(string name)
    {
      var index = name.LastIndexOf('<');
      return index != -1
        ? name.Remove(index)
        : name;
    }

    protected static string ResolveDocName(TypeSig source)
    {
      static IEnumerable<string> RetrieveNested(dnlib.DotNet.IType source)
      {
        static string SolveGenerics(string value)
        {
          var index = value.IndexOf('`', StringComparison.InvariantCultureIgnoreCase);
          if (index == -1)
            return value;

          var name = value.Remove(index);
          return int.TryParse(value.Substring(index + 1), out var number)
            ? $"{name}{{{string.Join(",", Enumerable.Repeat('`', number).Select((x,i) => $"{x}{i}"))}}}"
            : name;
        }

        if (source is null)
          yield break;
        var type = RetrieveNested(source.ScopeType.DeclaringType);
        foreach (var item in type)
          yield return item;

        if (source.ScopeType.DeclaringType is null)
          yield return source.Namespace;

        yield return SolveGenerics(source.Name);
      }

      if (source is null)
        throw new ArgumentNullException(nameof(source));

      if (!source.FullName.Contains('/', StringComparison.InvariantCultureIgnoreCase))
        return source.FullName;

      var result = $"{string.Join(".", RetrieveNested(source.ScopeType))}";
      return result;
    }

    protected static string ResolveName(dnlib.DotNet.IType source)
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));

      var name = source.Name.String;
      var genericsIndex = name.IndexOf('`', StringComparison.InvariantCulture);

      return genericsIndex != -1
        ? name.Remove(genericsIndex)
        : name;
    }

    #endregion
  }
}