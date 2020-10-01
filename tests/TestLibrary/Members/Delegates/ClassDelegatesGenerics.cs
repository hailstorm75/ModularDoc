// ReSharper disable All
using System;

namespace TestLibrary.Members.Delegates
{
  public class ClassDelegatesGenerics
  {
    public delegate void DelegateGeneric<TDelegate>();
    public delegate void DelegateGenericConstraint<TDelegate>() where TDelegate : IDisposable;
  }
}
