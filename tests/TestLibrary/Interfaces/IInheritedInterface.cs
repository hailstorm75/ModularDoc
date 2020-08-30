using System;

namespace TestLibrary.Interfaces
{
  public interface IInheritedInterface
  {
    event EventHandler Event;
    delegate void Delegate();
    string Property { get; set; }
    void Method();
  }
}