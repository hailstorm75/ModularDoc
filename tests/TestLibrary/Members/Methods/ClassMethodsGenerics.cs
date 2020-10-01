// ReSharper disable All
using System;

namespace TestLibrary.Members.Methods
{
  public class ClassMethodsGenerics
  {
    public void MethodGeneric<TMethod>() { }
    public void MethodGenericConstraint<TMethod>()
      where TMethod : IDisposable { }
  }
}