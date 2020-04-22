using System;
using System.Collections.Generic;

namespace MarkDoc.Members.Dnlib
{
  public class ClassDef
    : InterfaceDef, IClass
  {
    #region Properties

    /// <inheritdoc />
    public bool IsAbstract { get; }

    /// <inheritdoc />
    public Lazy<IClass?> BaseClass { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<Lazy<IClass>> NestedClasses { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IConstructor> Constructors { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public ClassDef()
      : base()
    {

    }
  }
}
