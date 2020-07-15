using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MarkDoc.Helpers;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Members;

namespace MarkDoc.Members.Dnlib.Members
{
  [DebuggerDisplay(nameof(ConstructorDef) + (": {" + nameof(Name) + "}"))]
  public class ConstructorDef
    : MemberDef, IConstructor
  {
    private static readonly char[] GENERIC_CHAR = { '`' };

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
    protected ConstructorDef(IResolver resolver, dnlib.DotNet.MethodDef source, string name)
      : base(resolver, source)
    {
      var generics = source.ResolveMethodGenerics();

      Name = name;
      IsStatic = source.IsStatic;
      Arguments = source.Parameters.Where(x => !string.IsNullOrEmpty(x.Name)).Select(x => new ArgumentDef(resolver, x, generics)).ToReadOnlyCollection();
      RawName = $"{(source.IsConstructor ? "#ctor" : name)}({string.Join(",", Arguments.Select(x => x.Type.DocumentationName))})";
      Accessor = ResolveAccessor(source);
    }

    #endregion

    #region Methods

    private static string ResolveName(dnlib.DotNet.MethodDef source, bool isNested)
    {
      ReadOnlySpan<char> CutNamespace()
      {
        var type = source.DeclaringType;
        var fullname = type.FullName.AsSpan();
        if (isNested)
          return fullname.Slice(type.FullName.LastIndexOf('/') + 1);

        return type.Namespace.Length != 0
          ? fullname.Slice(type.Namespace.Length + 1)
          : fullname;
      }

      var namespaceCut = CutNamespace();

      if (!source.DeclaringType.HasGenericParameters)
        return namespaceCut.ToString();

      var genericsIndex = namespaceCut.IndexOf(GENERIC_CHAR.AsSpan(), StringComparison.InvariantCulture);
      if (genericsIndex == -1)
        return namespaceCut.ToString();

      var genericCut = namespaceCut.Slice(0, genericsIndex);

      return genericCut.ToString();
    }

    private static AccessorType ResolveAccessor(dnlib.DotNet.MethodDef method)
    {
      if (method.Access == dnlib.DotNet.MethodAttributes.Public)
        return AccessorType.Public;
      if (method.Access == dnlib.DotNet.MethodAttributes.Family)
        return AccessorType.Protected;
      if (method.Access == dnlib.DotNet.MethodAttributes.Assembly)
        return AccessorType.Internal;

      throw new Exception("Private methods not allowed");
    }

    #endregion
  }
}