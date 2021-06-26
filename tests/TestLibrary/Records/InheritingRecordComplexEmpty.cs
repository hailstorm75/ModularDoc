// ReSharper disable All

using System;
using TestLibrary.Interfaces;

#pragma warning disable 67
namespace TestLibrary.Records
{
  public record InheritingRecordComplexEmpty
    : IInheritingAndInheritedInterface
  {
    public string OtherProperty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Property { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public event EventHandler OtherEvent;
    public event EventHandler Event;

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