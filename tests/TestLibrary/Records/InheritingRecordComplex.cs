// ReSharper disable All
#pragma warning disable 67
#pragma warning disable CS0414

using System;
using TestLibrary.Interfaces;

namespace TestLibrary.Records
{
  public record InheritingRecordComplex
    : IInheritingAndInheritedInterface
  {
    public event EventHandler MyEvent = null!;
    public event EventHandler OtherEvent = null!;
    public event EventHandler Event = null!;

    public delegate void MyDelegate();

    public string MyProperty { get; set; } = string.Empty;

    public string OtherProperty
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public string Property
    {
      get => throw new NotImplementedException();
      set => throw new NotImplementedException();
    }

    public void MyMethod()
    {
    }

    public void OtherMethod()
    {
      throw new NotImplementedException();
    }

    public void Method()
    {
      throw new NotImplementedException();
    }
  }
}