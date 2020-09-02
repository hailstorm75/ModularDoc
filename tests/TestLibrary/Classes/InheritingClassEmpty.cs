using System;
using TestLibrary.Interfaces;

namespace TestLibrary.Classes
{
  public class InheritingClassEmpty
    : IInheritedInterface
  {
    public string Property { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public event EventHandler Event = null!;

    public void Method()
    {
      throw new NotImplementedException();
    }
  }
}
