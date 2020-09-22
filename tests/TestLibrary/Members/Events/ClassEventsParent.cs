#pragma warning disable 67
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
// ReSharper disable All
using System;

namespace TestLibrary.Members.Events
{
  public class ClassEventsParent
  {
    public event EventHandler EventTop;

    public class ClassEventsNested
    {
      public event EventHandler EventNested;

      public class ClassEventsNestedNested
      {
        public event EventHandler EventNestedNested;
      }
    }
  }
}
