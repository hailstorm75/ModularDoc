using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ModularDoc.Helpers;
using ModularDoc.Members.Dnlib.Helpers;
using ModularDoc.Members.Dnlib.Properties;
using ModularDoc.Members.Enums;
using ModularDoc.Members.Members;

namespace ModularDoc.Members.Dnlib.Members
{
  /// <summary>
  /// Class for representing constructor members
  /// </summary>
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
    internal ConstructorDef(Resolver resolver, dnlib.DotNet.MethodDef source, bool isNested)
      : this(resolver, source, ResolveName(source, isNested)) { }

    /// <summary>
    /// Inherited constructor
    /// </summary>
    protected ConstructorDef(Resolver resolver, dnlib.DotNet.MethodDef source, string name)
      : base(resolver, source)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));
      // If the name is null..
      if (name is null)
        // throw an exception
        throw new ArgumentNullException(nameof(name));

      Name = name;
      IsStatic = source.IsStatic;
      Arguments = ResolveArguments(resolver, source).ToReadOnlyCollection();
      var rawName = source.FullName.Substring(source.FullName.IndexOf(' ', StringComparison.InvariantCulture) + 1).Replace("::", ".", StringComparison.InvariantCulture);
      RawName = source.IsConstructor
        ? rawName.Replace(".ctor(", "#ctor(", StringComparison.InvariantCulture)
        : rawName;
      Accessor = ResolveAccessor(source);
    }

    #endregion

    #region Methods

    private static IEnumerable<IArgument> ResolveArguments(Resolver resolver, dnlib.DotNet.MethodDef method)
      => method.Parameters
        // Filter out invalid arguments
        .Where(parameter => !string.IsNullOrEmpty(parameter.Name))
        // Initialize the arguments
        .Select(parameter => new ArgumentDef(resolver, parameter, method.ResolveMethodGenerics()));

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
      => method.Access switch
      {
        dnlib.DotNet.MethodAttributes.Public => AccessorType.Public,
        dnlib.DotNet.MethodAttributes.Family => AccessorType.Protected,
        dnlib.DotNet.MethodAttributes.Assembly => AccessorType.Internal,
        dnlib.DotNet.MethodAttributes.FamORAssem => AccessorType.ProtectedInternal,
        _ => throw new NotSupportedException(Resources.accessorPrivate)
      };

    #endregion
  }
}