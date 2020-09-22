// ReSharper disable All
using System;

namespace TestLibrary.Interfaces
{
  public interface IInheritingInterface
    : IInheritedInterface
  {
    event EventHandler MyEvent;
    delegate void MyDelegate();
    string MyProperty { get; set; }
    void MyMethod();
  }
}