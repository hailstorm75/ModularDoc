using MarkDoc.Members.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using dnlib.DotNet;

namespace MarkDoc.Members.Dnlib
{
  [DebuggerDisplay(nameof(MethodDef) + ": {Name}")]
  public class MethodDef
    : ConstructorDef, IMethod
  {
    #region Properties

    /// <inheritdoc />
    public bool IsAsync { get; }

    /// <inheritdoc />
    public MemberInheritance Inheritance { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<string> Generics { get; }

    /// <inheritdoc />
    public IResType? Returns { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    internal MethodDef(dnlib.DotNet.MethodDef source)
      : base(source, ResolveName(source))
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      IsAsync = ResolveAsync(source);
      Inheritance = ResolveInheritance(source);
      Generics = ResolveGenerics(source).ToArray();
      Returns = ResolveReturn(source);
    }

    #region Methods

    private static string ResolveName(dnlib.DotNet.MethodDef source)
      => source.Name;

    private static bool ResolveAsync(dnlib.DotNet.MethodDef source)
    {
      // TODO: Check
      return source.CustomAttributes.Find(nameof(AsyncStateMachineAttribute)) != null;
    }

    private static IResType? ResolveReturn(dnlib.DotNet.MethodDef source)
    {
      if (source.ReturnType.TypeName.Equals("System.Void", StringComparison.InvariantCultureIgnoreCase))
        return null;
      return Resolver.Instance.Resolve(source.ReturnType);
    }

    private static MemberInheritance ResolveInheritance(dnlib.DotNet.MethodDef source)
    {
      if (source.IsVirtual && (source.Attributes & dnlib.DotNet.MethodAttributes.NewSlot) == 0)
        return MemberInheritance.Override;
      else if (source.IsVirtual)
        return MemberInheritance.Virtual;
      else if (source.IsAbstract)
        return MemberInheritance.Abstract;

      return MemberInheritance.Normal;
    }

    private static IEnumerable<string> ResolveGenerics(dnlib.DotNet.MethodDef source)
      => !source.HasGenericParameters
         ? Enumerable.Empty<string>()
         : source.GenericParameters.Select(x => x.Name.String);

    #endregion
  }
}
