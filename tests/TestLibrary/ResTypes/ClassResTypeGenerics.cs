using System;
using System.Collections.Generic;
using System.Linq;

namespace TestLibrary.ResTypes
{
  public class ClassResTypeGenerics
  {
    public IEnumerable<string> MethodGenericString() => Enumerable.Empty<string>();
    public IReadOnlyDictionary<object, dynamic> MethodGenericObjDyn() => new Dictionary<object, dynamic>();
    public IReadOnlyDictionary<dynamic, object> MethodGenericDynObj() => new Dictionary<dynamic, object>();
    public Func<int, string, bool> MethodGenericIntStringBool() => new Func<int, string, bool>((i, s) => default);
    public IReadOnlyDictionary<IReadOnlyDictionary<object, object>, IReadOnlyDictionary<IReadOnlyDictionary<object, object>, dynamic>> MethodGenericComplexAReturn() => new Dictionary<IReadOnlyDictionary<object, object>, IReadOnlyDictionary<IReadOnlyDictionary<object, object>, dynamic>>();
    public IReadOnlyDictionary<IReadOnlyDictionary<dynamic, dynamic>, IReadOnlyDictionary<IReadOnlyDictionary<dynamic, dynamic>, dynamic>> MethodGenericComplexBReturn() => new Dictionary<IReadOnlyDictionary<dynamic, dynamic>, IReadOnlyDictionary<IReadOnlyDictionary<dynamic, dynamic>, dynamic>>();
    public IReadOnlyDictionary<IReadOnlyDictionary<object, dynamic>, IReadOnlyDictionary<IReadOnlyDictionary<object, dynamic>, dynamic>> MethodGenericComplexCReturn() => new Dictionary<IReadOnlyDictionary<object, dynamic>, IReadOnlyDictionary<IReadOnlyDictionary<object, dynamic>, dynamic>>();
    public IReadOnlyDictionary<IReadOnlyDictionary<dynamic, object>, IReadOnlyDictionary<IReadOnlyDictionary<dynamic, object>, dynamic>> MethodGenericComplexDReturn() => new Dictionary<IReadOnlyDictionary<dynamic, object>, IReadOnlyDictionary<IReadOnlyDictionary<dynamic, object>, dynamic>>();
    public IReadOnlyDictionary<IReadOnlyDictionary<object, object>, IReadOnlyDictionary<IReadOnlyDictionary<object, object>, object>> MethodGenericComplexEReturn() => new Dictionary<IReadOnlyDictionary<object, object>, IReadOnlyDictionary<IReadOnlyDictionary<object, object>, object>>();
    public IReadOnlyDictionary<IReadOnlyDictionary<dynamic, dynamic>, IReadOnlyDictionary<IReadOnlyDictionary<dynamic, dynamic>, object>> MethodGenericComplexFReturn() => new Dictionary<IReadOnlyDictionary<dynamic, dynamic>, IReadOnlyDictionary<IReadOnlyDictionary<dynamic, dynamic>, object>>();
    public IReadOnlyDictionary<IReadOnlyDictionary<object, dynamic>, IReadOnlyDictionary<IReadOnlyDictionary<object, dynamic>, object>> MethodGenericComplexGReturn() => new Dictionary<IReadOnlyDictionary<object, dynamic>, IReadOnlyDictionary<IReadOnlyDictionary<object, dynamic>, object>>();
    public IReadOnlyDictionary<IReadOnlyDictionary<dynamic, object>, IReadOnlyDictionary<IReadOnlyDictionary<dynamic, object>, object>> MethodGenericComplexHReturn() => new Dictionary<IReadOnlyDictionary<dynamic, object>, IReadOnlyDictionary<IReadOnlyDictionary<dynamic, object>, object>>();
  }
}