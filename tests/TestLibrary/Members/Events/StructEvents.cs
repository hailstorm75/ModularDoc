#pragma warning disable CS0414
#pragma warning disable 67
using System;

namespace TestLibrary.Members.Events
{
  public struct StructEvents
  {
    public event EventHandler EventPublic;
    internal event EventHandler EventInternal;
  }
}
