#pragma warning disable 67
#pragma warning disable 414
// ReSharper disable All
using System;

namespace TestLibrary.Classes
{
  public class InheritingClassWithBase
    : ClassTypePublicAbstract
  {
    public delegate void MyDelegate();

    public string MyProperty { get; set; } = null!;
    public event EventHandler MyEvent = null!;
    public void MyMethod()
    {
      throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override string AbstractProperty { get; set; } = null!;

    /// <inheritdoc />
    public override event EventHandler AbstractEvent = null!;

    /// <inheritdoc />
    public override void AbstractMethod()
    {
      throw new NotImplementedException();
    }

  }
}
