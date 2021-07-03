// ReSharper disable All

using System;
using TestLibrary.Interfaces;

#pragma warning disable 67
#pragma warning disable CS0414

namespace TestLibrary.Records
{
  public record InheritingRecordComplexEmpty
    : IInheritingAndInheritedInterface
  {
    public string OtherProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Property { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public event EventHandler OtherEvent = null!;
    public event EventHandler Event = null!;

    public void Method()
    {
      throw new NotImplementedException();
    }

    public void OtherMethod()
    {
      throw new NotImplementedException();
    }
  }
}