#pragma warning disable 67
using System;

namespace TestLibrary.Structs
{
  public struct StructTypePublic
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

    public delegate string Delegate(int arg);

    public event EventHandler Event;

    public string Property { get; set; }

    public void Method()
    {
    }
  }
}