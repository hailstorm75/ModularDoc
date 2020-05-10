using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Linq;
using MarkDoc.Helpers;

namespace MarkDoc.Members.Dnlib
{
  [DebuggerDisplay(nameof(ClassDef) + ": {Name}")]
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

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    internal ClassDef(IResolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent)
      : base(resolver, source, parent)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      BaseClass = ResolveBaseClass(source);
      IsAbstract = source.IsAbstract;
      Constructors = source.Methods.Where(x => !x.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Getter)
                                            && !x.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Setter)
                                            && x.IsConstructor)
                                   .Select(x => new ConstructorDef(resolver, x, parent != null))
                                   .ToReadOnlyCollection();
    }

    private IResType? ResolveBaseClass(dnlib.DotNet.TypeDef source)
      => source.BaseType?.FullName.Equals("System.Object", StringComparison.InvariantCulture) == false
          ? Resolver.Resolve(source.BaseType.ToTypeSig())
          : null;
  }
}
