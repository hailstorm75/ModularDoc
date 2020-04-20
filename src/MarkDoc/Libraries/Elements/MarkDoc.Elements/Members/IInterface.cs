using System.Collections.Generic;

namespace MarkDoc.Elements.Members
{
  public interface IInterface
    : IType
  {
    public new bool IsStatic
      => false;

    public enum Variance
    {
      Contravariance,
      Covariance,
      Variance
    }

    IReadOnlyCollection<IInterface> InheritedInterfaces { get; }
    IReadOnlyDictionary<string, (Variance variance, IReadOnlyCollection<IType> constraints)> Generics { get; }

    IReadOnlyCollection<IEnum> NestedEnums { get; }
    IReadOnlyCollection<IEvent> Events { get; }
    IReadOnlyCollection<IMethod> Methods { get; }
    IReadOnlyCollection<IProperty> Properties { get; }
  }
}
