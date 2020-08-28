using System;

namespace TestLibrary.Interfaces
{
  public interface IInheritingInterfaceComplex
    : IInheritingAndInheritedInterface
  {
    event EventHandler MyEvent;
    delegate void MyDelegate();
    string MyProperty { get; set; }
    void MyMethod();
  }
}