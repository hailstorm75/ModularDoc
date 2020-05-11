using dnlib.DotNet;
using MarkDoc.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkDoc.Members.Dnlib
{
  public static class GenericsHelper
  {
    public static IReadOnlyDictionary<string, string> ResolveMethodGenerics(this dnlib.DotNet.MethodDef source)
    {
      static IEnumerable<(string type, string name)> ResolveParentTypeGenerics(dnlib.DotNet.MethodDef source)
        => GetGenericArgumeents(source.DeclaringType)
          .DistinctBy(x => x.Name)
          .Select((x, i) => (x.Name.String, $"`{i}"));

      static IEnumerable<(string type, string name)> ResolveTypeGenerics(dnlib.DotNet.MethodDef source)
        => source.GenericParameters.Select((x, i) => (x.Name.String, $"``{i}"));

      if (source == null)
        throw new ArgumentNullException(nameof(source));

      var outerArgs = ResolveParentTypeGenerics(source);
      var thisArgs = ResolveTypeGenerics(source);
      return outerArgs.Concat(thisArgs).ToDictionary(x => x.type, x => x.name);
    }

    public static IReadOnlyDictionary<string, string> ResolvePropertyGenerics(this dnlib.DotNet.PropertyDef source, IReadOnlyCollection<dnlib.DotNet.MethodDef> methods)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      var outerArgs = GetGenericArgumeents(source.DeclaringType)
        .DistinctBy(x => x.Name)
        .Select((x, i) => new { Type = x.Name, Name = $"`{i}" });
      var thisArgs = methods.Select(x => x.GenericParameters)
        .SelectMany(Linq.XtoX)
        .DistinctBy(x => x.Name)
        .Select((x, i) => new { Type = x.Name, Name = $"``{i}" });
      return outerArgs.Concat(thisArgs).ToDictionary(x => x.Type.String, x => x.Name);
    }

    public static IReadOnlyDictionary<string, string> ResolveTypeGenerics(this dnlib.DotNet.TypeDef source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      return GetGenericArgumeents(source.DeclaringType)
        .DistinctBy(x => x.Name)
        .Select((x, i) => new { Type = x.Name, Name = $"`{i}" })
        .ToDictionary(x => x.Type.String, x => x.Name);
    }

    private static IEnumerable<GenericParam> GetGenericArgumeents(dnlib.DotNet.TypeDef? type)
    {
      if (type == null)
        yield break;

      foreach (var parameter in GetGenericArgumeents(type.DeclaringType))
        yield return parameter;
      foreach (var parameter in type.GenericParameters)
        yield return parameter;
    }
  }
}
