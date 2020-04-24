using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;
using MarkDoc.Members.Enums;

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

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    internal ConstructorDef(dnlib.DotNet.MethodDef source, bool isNested)
      : this(source, ResolveName(source, isNested)) { }

    /// <summary>
    /// Inherited constructor
    /// </summary>
    protected ConstructorDef(dnlib.DotNet.MethodDef source, string name)
      : base(source)
    {
      Name = name;
      IsStatic = source.IsStatic;
      Arguments = source.Parameters.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => new ArgumentDef(x)).ToArray();
      Accessor = ResolveAccessor(source);
    } 

    #endregion

    #region Methods

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
        ? source.FullName.Substring(type.Namespace.Length + 1)
        : source.FullName;
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