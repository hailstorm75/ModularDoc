// ReSharper disable All

using System;
using TestLibrary.Interfaces;

#pragma warning disable 67
namespace TestLibrary.Records
{
  public record InheritingRecordEmpty
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