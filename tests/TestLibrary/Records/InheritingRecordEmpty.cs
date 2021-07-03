// ReSharper disable All

using System;
using TestLibrary.Interfaces;

#pragma warning disable 67
#pragma warning disable CS0414

namespace TestLibrary.Records
{
  public record InheritingRecordEmpty
    : IInheritedInterface
  {
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