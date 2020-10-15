using System.Collections.Generic;
using System.Linq;

namespace TestLibrary.ResTypes
{
  public class ClassRestTypeGenerics
  {
    public IEnumerable<string> MethodGenericString() => Enumerable.Empty<string>();
  }
}