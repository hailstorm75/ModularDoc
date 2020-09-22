#pragma warning disable 67
// ReSharper disable All
using System;

namespace TestLibrary.Members.Events
{
  public abstract class ClassEventsAbstract
    : ClassEventsBase
  {
    public event EventHandler EventNormal = null!;
    public abstract event EventHandler EventAbstract;
    public virtual event EventHandler EventVirtual = null!;
    public override event EventHandler EventOverride = null!;
  }
}
