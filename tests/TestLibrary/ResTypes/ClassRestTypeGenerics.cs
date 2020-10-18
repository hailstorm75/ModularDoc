using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TestLibrary.ResTypes
{
  public class ClassRestTypeGenerics
  {
    public IEnumerable<string> MethodGenericString() => Enumerable.Empty<string>();
    public IReadOnlyDictionary<object, dynamic> MethodGenericObjDyn() => new Dictionary<object, dynamic>();
    public IReadOnlyDictionary<dynamic, object> MethodGenericDynObj() => new Dictionary<dynamic, object>();
    public Func<int, string, bool> MethodGenericIntStringBool() => new Func<int, string, bool>((i, s) => default);
  }
}