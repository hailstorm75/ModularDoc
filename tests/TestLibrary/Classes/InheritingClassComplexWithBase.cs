using System;
using TestLibrary.Interfaces;

namespace TestLibrary.Classes
{
  public class InheritingClassComplexWithBase
    : ClassTypePublicAbstract, IInheritingAndInheritedInterface
  {
    public delegate void MyDelegate();
    public string OtherProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Property { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public event EventHandler OtherEvent = null!;
    public event EventHandler Event = null!;

    public void Method()
    {
      throw new NotImplementedException();
    }

    public void OtherMethod()
    {
      throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override string AbstractProperty { get; set; } = null!;

    /// <inheritdoc />
    public override event EventHandler AbstractEvent = null!;

    /// <inheritdoc />
    public override void AbstractMethod()
    {
      throw new NotImplementedException();
    }
  }
}
