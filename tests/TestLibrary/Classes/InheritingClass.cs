// ReSharper disable all
#pragma warning disable 67
using System;
using TestLibrary.Interfaces;

namespace TestLibrary.Classes
{
  public class InheritingClass
    : IInheritedInterface
  {
    public delegate void MyDelegate();

    public event EventHandler MyEvent = null!;
    /// <inheritdoc />
    public event EventHandler Event = null!;

    public string MyProperty { get; set; } = null!;

    /// <inheritdoc />
    public string Property { get; set; } = null!;

    public void MyMethod() {}
    /// <inheritdoc />
    public void Method()
    {
      throw new NotImplementedException();
    }
  }
}
