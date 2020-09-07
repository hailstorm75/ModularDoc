#pragma warning disable 414
using System;
using TestLibrary.Interfaces;

namespace TestLibrary.Classes
{
  public class InheritingClassComplexEmpty
    : IInheritingAndInheritedInterface
  {
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
  }
}
