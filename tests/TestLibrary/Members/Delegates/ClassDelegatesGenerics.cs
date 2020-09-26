using System;

namespace TestLibrary.Members.Delegates
{
  public class ClassDelegatesGenerics
  {
    public delegate void DelegateGeneric<TDelegate>();
    public delegate void DelegateGenericContraint<TDelegate>() where TDelegate : IDisposable;
  }
}
