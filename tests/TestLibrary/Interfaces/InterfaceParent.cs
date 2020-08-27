// ReSharper disable All
using System;

namespace TestLibrary.Interfaces
{
  public class InterfaceParent
  {
    public interface INestedInterfacePublic
    {
    }

    internal interface INestedInterfaceInternal
    {
    }

    protected interface INestedInterfaceProtected
    {
    }

    public interface INestedGenericInterface<T1, in T2, out T3>
      where T2 : IInterfaceTypePublic
      where T3 : IDisposable
    {
    }
  }
}
