using System;
using System.IO;
using TestLibrary.Classes;

namespace TestLibrary.ResTypes
{
  public class ClassResTypes
  {
    public class NestedResTypesGeneric<T>
    {
      public class NestedNestedResTypesGeneric
      {
      }
    }

    public NestedResTypesGeneric<T>.NestedNestedResTypesGeneric MethodNestedGenericParent<T>() => default!;
    public ClassParent.NestedGenericClass<T1, T2> MethodNestedGenericOwnValueTypes<T1, T2>() where T2 : IDisposable=> default!;
    public ClassParent.NestedGenericClass<int, StreamReader> MethodNestedGenericRet() => default!;
    public ClassParent.NestedClassPublic MethodNestedRet() => default!;
  }
}