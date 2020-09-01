// ReSharper disable All
#pragma warning disable 67
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
using System;

namespace TestLibrary.Classes
{
  public class ClassTypePublic
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

    public string Property { get; set; } = string.Empty;

    public void Method()
    {

    }
  }
}
