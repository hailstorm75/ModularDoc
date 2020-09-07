#pragma warning disable 67
#pragma warning disable 414
using System;
using TestLibrary.Interfaces;

namespace TestLibrary.Classes
{
  public class InheritingClassComplex
    : IInheritingAndInheritedInterface
  {
    public delegate void MyDelegate();
    public string OtherProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string MyProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Property { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public event EventHandler OtherEvent = null!;
    public event EventHandler MyEvent = null!;
    public event EventHandler Event = null!;

    public void MyMethod()
    {
      throw new NotImplementedException();
    }

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
