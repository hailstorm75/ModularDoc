// ReSharper disable All
using System;

namespace TestLibrary.Interfaces
{
  public interface IGenericInterface<T1, in T2, out T3>
    where T2 : IInterfaceTypePublic
    where T3 : IDisposable
  {
  }
}