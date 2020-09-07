#pragma warning disable 67
#pragma warning disable 414
// ReSharper disable all
using System;

namespace TestLibrary.Classes
{
  public abstract class ClassTypePublicAbstract
  {
    public delegate void BaseDelegate();
    public string BaseProperty { get; set; } = null!;
    public event EventHandler BaseEvent = null!;
    public void BaseMethod() { }

    public virtual string VirtualProperty { get; set; } = null!;
    public virtual event EventHandler VirtualEvent = null!;
    public virtual void VirtualMethod() { }

    public abstract string AbstractProperty { get; set; }
    public abstract event EventHandler AbstractEvent;
    public abstract void AbstractMethod();
  }
}
