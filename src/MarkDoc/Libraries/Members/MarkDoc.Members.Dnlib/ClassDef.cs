using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Linq;

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
    public Lazy<IClass?> BaseClass { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IConstructor> Constructors { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public ClassDef(dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent)
      : base(source, parent)
    {
      // TODO: Implement type resolver
      BaseClass = new Lazy<IClass?>(() => default, LazyThreadSafetyMode.ExecutionAndPublication);
      IsAbstract = source.IsAbstract;
      Constructors = source.Methods.Where(x => !x.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Getter)
                                            && !x.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Setter)
                                            && x.IsConstructor)
                                   .Select(x => new ConstructorDef(x, parent != null))
                                   .ToArray();
    }
  }
}
