// ReSharper disable All
#pragma warning disable 67
using System;
using TestLibrary.Interfaces;

namespace TestLibrary.Structs
{
  public struct InheritingStructComplex
    : IInheritingAndInheritedInterface
  {
    public event EventHandler MyEvent;
    public event EventHandler OtherEvent;
    public event EventHandler Event;

    public delegate void MyDelegate();
    public string MyProperty { get; set; }
    public string OtherProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Property { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void MyMethod() {}

    public void OtherMethod()
    {
      throw new NotImplementedException();
    }

    public void Method()
    {
      throw new NotImplementedException();
    }
  }
}
