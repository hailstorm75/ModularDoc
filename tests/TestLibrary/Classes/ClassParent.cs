// ReSharper disable All
using System;

namespace TestLibrary.Classes
{
  public class ClassParent
  {
    public class NestedClassPublic
    {
    }

    internal class NestedClassInternal
    {
    }

    protected class NestedClassProtected
    {
    }

    public class NestedGenericClass<T1, T2>
      where T2 : IDisposable
    {
    }
  }
}
