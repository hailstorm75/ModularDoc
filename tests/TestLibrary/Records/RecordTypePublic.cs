// ReSharper disable All

using System;

#pragma warning disable 67
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

    public event EventHandler Event;

    public string Property { get; set; }

    public void Method()
    {
    }
  }
}