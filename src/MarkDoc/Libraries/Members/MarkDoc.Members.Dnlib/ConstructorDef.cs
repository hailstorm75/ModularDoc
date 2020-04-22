using System.Collections.Generic;
using System.Linq;

namespace MarkDoc.Members.Dnlib
{
  public class ConstructorDef
    : MemberDef, IConstructor
  {
    #region Properties

    /// <inheritdoc />
    public override bool IsStatic { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IArgument> Arguments { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public ConstructorDef(dnlib.DotNet.MethodDef source)
      : base(source)
    {
      IsStatic = source.IsStatic;
      Arguments = source.Parameters.Select(x => new ArgumentDef(x)).ToArray();
    }
  }
}