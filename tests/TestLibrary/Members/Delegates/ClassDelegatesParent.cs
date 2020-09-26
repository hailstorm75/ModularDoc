using System;

namespace TestLibrary.Members.Delegates
{
  public class ClassDelegatesParent
  {
    public delegate void DelegateTop();

    public class ClassDelegatesNested
    {
      public delegate void DelegateNested();

      public class ClassDelegatesNestedNested
      {
        public delegate void DelegateNestedNested();
      }
    }
  }
}
