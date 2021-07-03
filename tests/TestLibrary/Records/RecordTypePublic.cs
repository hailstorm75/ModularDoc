// ReSharper disable All

using System;

#pragma warning disable 67
#pragma warning disable CS0414

namespace TestLibrary.Records
{
  public record RecordTypePublic
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

    public record MyRecord
    {
    }

    public delegate string Delegate(int arg);

    public event EventHandler Event = null!;

    public string Property { get; set; } = string.Empty;

    public void Method()
    {
    }
  }
}