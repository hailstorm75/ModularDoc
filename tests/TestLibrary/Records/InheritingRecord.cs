// ReSharper disable All
#pragma warning disable 67
#pragma warning disable CS0414

using System;
using TestLibrary.Interfaces;

namespace TestLibrary.Records
{
  public record InheritingRecord
    : IInheritedInterface
  {
    public event EventHandler MyEvent = null!;
    public delegate void MyDelegate();
    public string MyProperty { get; set; } = string.Empty;
    public void MyMethod() {}

    /// <inheritdoc />
    public event EventHandler Event = null!;

    /// <inheritdoc />
    public string Property { get; set; } = string.Empty;

    /// <inheritdoc />
    public void Method()
    {
      throw new NotImplementedException();
    }
  }
}