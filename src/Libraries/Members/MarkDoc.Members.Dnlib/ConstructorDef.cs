using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;
using MarkDoc.Members.Enums;
using MarkDoc.Helpers;
using dnlib.DotNet;

namespace MarkDoc.Members.Dnlib
{
  [DebuggerDisplay(nameof(ConstructorDef) + ": {Name}")]
  public class ConstructorDef
    : MemberDef, IConstructor
  {
    #region Properties

    /// <inheritdoc />
    public override string Name { get; }

    /// <inheritdoc />
    public override bool IsStatic { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IArgument> Arguments { get; }

    /// <inheritdoc />
    public override AccessorType Accessor { get; }

    public override string RawName { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    internal ConstructorDef(IResolver resolver, dnlib.DotNet.MethodDef source, bool isNested)
      : this(resolver, source, ResolveName(source, isNested)) { }

    /// <summary>
    /// Inherited constructor
    /// </summary>
    internal protected ConstructorDef(IResolver resolver, dnlib.DotNet.MethodDef source, string name)
      : base(resolver, source)
    {
      var outerArgs = GetGenericArgumeents(source.DeclaringType).DistinctBy(x => x.Name).Select((x, i) => new { Type = x.Name, Name = $"`{i}" });
      var thisArgs = source.GenericParameters.Select((x, i) => new { Type = x.Name, Name = $"``{i}" });
      var generics = outerArgs.Concat(thisArgs).ToDictionary(x => x.Type.String, x => x.Name);

      Name = name;
      IsStatic = source.IsStatic;
      Arguments = source.Parameters.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => new ArgumentDef(resolver, x, generics)).ToReadOnlyCollection();
      RawName = $"{Name}({string.Join(",", Arguments.Select(x => x.Type.Name))})";
      Accessor = ResolveAccessor(source);
    }

    #endregion

    #region Methods

    private IEnumerable<GenericParam> GetGenericArgumeents(dnlib.DotNet.TypeDef? type)
    {
      if (type == null)
        yield break;

      foreach (var parameter in GetGenericArgumeents(type.DeclaringType))
        yield return parameter;
      foreach (var parameter in type.GenericParameters)
        yield return parameter;
    }

    private static string ResolveName(dnlib.DotNet.MethodDef source, bool isNested)
    {
      var namespaceCut = CutNamespace(source, isNested);

      if (!source.DeclaringType.HasGenericParameters)
        return namespaceCut;

      var genericsIndex = namespaceCut.IndexOf('`', StringComparison.InvariantCulture);
      if (genericsIndex == -1)
        return namespaceCut;

      var genericCut = namespaceCut.Remove(genericsIndex);

      return genericCut;
    }

    private static string CutNamespace(dnlib.DotNet.MethodDef source, bool isNested)
    {
      var type = source.DeclaringType;
      if (isNested)
        return type.FullName.Substring(type.FullName.IndexOf('/', StringComparison.InvariantCulture) + 1);

      return type.Namespace.Length != 0
        ? type.FullName.Substring(type.Namespace.Length + 1)
        : type.FullName;
    }

    private static AccessorType ResolveAccessor(dnlib.DotNet.MethodDef method)
    {
      if (method.Access == dnlib.DotNet.MethodAttributes.Public)
        return AccessorType.Public;
      if (method.Access == dnlib.DotNet.MethodAttributes.Family)
        return AccessorType.Protected;
      return AccessorType.Internal;
    }

    #endregion
  }
}