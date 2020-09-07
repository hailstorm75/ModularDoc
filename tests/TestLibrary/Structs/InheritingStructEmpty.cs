#pragma warning disable 67
// ReSharper disable All
using System;
using TestLibrary.Interfaces;

namespace TestLibrary.Structs
{
  public struct InheritingStructEmpty
    : IInheritedInterface
  {
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
