using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;

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

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    internal ConstructorDef(dnlib.DotNet.MethodDef source)
      : this(source, ResolveName(source)) { }

    /// <summary>
    /// Inherited constructor
    /// </summary>
    protected ConstructorDef(dnlib.DotNet.MethodDef source, string name)
      : base(source)
    {
      Name = name;
      IsStatic = source.IsStatic;
      Arguments = source.Parameters.Select(x => new ArgumentDef(x)).ToArray();
    }

    private static string ResolveName(dnlib.DotNet.MethodDef source)
    {
      var type = source.DeclaringType;
      var namespaceCut = type.FullName.Substring(type.Namespace.Length + 1);

      if (!type.HasGenericParameters)
        return namespaceCut;

      return namespaceCut.Remove(namespaceCut.IndexOf('`', StringComparison.InvariantCulture));
    }
  }
}