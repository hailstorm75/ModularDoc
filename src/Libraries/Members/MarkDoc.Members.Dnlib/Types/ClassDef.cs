using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using MarkDoc.Helpers;
using MarkDoc.Members.Dnlib.Members;
using MarkDoc.Members.Members;
using MarkDoc.Members.ResolvedTypes;
using MarkDoc.Members.Types;

namespace MarkDoc.Members.Dnlib.Types
{
  [DebuggerDisplay(nameof(ClassDef) + (": {" + nameof(Name) + "}"))]
  public class ClassDef
    : InterfaceDef, IClass
  {
    #region Properties

    /// <inheritdoc />
    public bool IsAbstract { get; }

    /// <inheritdoc />
    public IResType? BaseClass { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IConstructor> Constructors { get; }

    /// <inheritdoc />
    public bool IsStatic { get; }

    /// <inheritdoc />
    public bool IsSealed { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    internal ClassDef(IResolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent)
      : base(resolver, source, parent)
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));
      BaseClass = ResolveBaseClass(source);
      Constructors = source.Methods.Where(x => !x.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Getter)
                                            && !x.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Setter)
                                            && x.IsConstructor)
                                   .Select(x => new ConstructorDef(resolver, x, parent != null))
                                   .ToReadOnlyCollection();

      IsStatic = source.IsSealed && source.IsAbstract;
      if (IsStatic)
      {
        IsSealed = false;
        IsAbstract = false;
      }
      else
      {
        IsSealed = source.IsSealed;
        IsAbstract = source.IsAbstract;
      }
    }

    private IResType? ResolveBaseClass(dnlib.DotNet.TypeDef source)
      => source.BaseType?.FullName.Equals("System.Object", StringComparison.InvariantCulture) == false
          ? Resolver.Resolve(source.BaseType.ToTypeSig(), source.ResolveTypeGenerics())
          : null;
  }
}
