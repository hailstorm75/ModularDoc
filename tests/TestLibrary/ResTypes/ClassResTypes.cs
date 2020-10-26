using System.IO;
using TestLibrary.Classes;

namespace TestLibrary.ResTypes
{
  public class ClassResTypes
  {
    public ClassParent.NestedGenericClass<int, StreamReader> MethodNestedGenericRet() => new ClassParent.NestedGenericClass<int, StreamReader>();
    public ClassParent.NestedClassPublic MethodNestedRet() => new ClassParent.NestedClassPublic();
  }
}