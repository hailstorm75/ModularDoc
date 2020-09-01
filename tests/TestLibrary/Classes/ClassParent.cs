// ReSharper disable All
using System;

namespace TestLibrary.Classes
{
  public class ClassParent
  {
    public struct NestedClassPublic
    {
    }

    internal struct NestedClassInternal
    {
    }

    protected struct NestedClassProtected
    {
    }

    public struct NestedGenericClass<T1, T2>
      where T2 : IDisposable
    {
    }
  }
}
