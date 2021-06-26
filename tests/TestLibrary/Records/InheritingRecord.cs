// ReSharper disable All
#pragma warning disable 67
using System;
using TestLibrary.Interfaces;

namespace TestLibrary.Records
{
  public record InheritingRecord
    : IInheritedInterface
  {
    public event EventHandler MyEvent;
    public delegate void MyDelegate();
    public string MyProperty { get; set; }
    public void MyMethod() {}

    /// <inheritdoc />
    public event EventHandler Event;

    /// <inheritdoc />
    public string Property { get; set; }

    /// <inheritdoc />
    public void Method()
    {
      throw new NotImplementedException();
    }
  }
}