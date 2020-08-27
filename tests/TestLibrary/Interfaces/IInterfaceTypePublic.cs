using System;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace TestLibrary.Interfaces
{
  public interface IInterfaceTypePublic
    : IDisposable
  {
    public enum MyEnum
    {
      Field
    }

    public class MyClass
    {
    }

    public struct MyStruct
    {
    }

    public interface IMyInterface
    {
    }

    delegate string Delegate(int arg);

    event EventHandler Event;

    string Property { get; set; }

    void Method();
  }
}
