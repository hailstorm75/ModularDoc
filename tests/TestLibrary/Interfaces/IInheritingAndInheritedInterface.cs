// ReSharper disable All
using System;

namespace TestLibrary.Interfaces
{
  public interface IInheritingAndInheritedInterface
    : IInheritedInterface
  {
    event EventHandler OtherEvent;
    delegate void OtherDelegate();
    string OtherProperty { get; set; }
    void OtherMethod();
  }
}