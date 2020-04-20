using System;
using System.Collections.Generic;
using System.Text;

namespace MarkDoc.Elements.Members
{
  public interface IClass
    : IInterface
  {
    IClass? BaseClass { get; }

    IReadOnlyCollection<IClass> NestedClasses { get; }
  }
}
