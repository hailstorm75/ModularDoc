using dnlib.DotNet;
using MarkDoc.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkDoc.Members.Dnlib
{
  /// <summary>
  /// Helper methods for extracting generics
  /// </summary>
  public static class GenericsHelper
  {
    /// <summary>
    /// Resolves generics for given <paramref name="source"/>
    /// </summary>
    /// <param name="source">Method to process</param>
    /// <returns>Resolved pairs for generic display names and raw names</returns>
    public static IReadOnlyDictionary<string, string> ResolveMethodGenerics(this MethodDef source)
    {
      // Resolves generics of parent generics
      static IEnumerable<(string type, string name)> ResolveParentTypeGenerics(MethodDef source)
        => GetGenericArguments(source.DeclaringType)
            // Distinct by name
            .DistinctBy(x => x.Name)
            // Pair generics with their display name and raw name
            .Select((x, i) => (x.Name.String, $"`{i}"));

      // Resolves generics of this method
      static IEnumerable<(string type, string name)> ResolveTypeGenerics(MethodDef source)
        => source.GenericParameters.Select((x, i) => (x.Name.String, $"``{i}"));

      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      // Resolve declaring parent generics
      var outerArgs = ResolveParentTypeGenerics(source);
      // Resolve this method generics
      var thisArgs = ResolveTypeGenerics(source);

      return outerArgs
        // Join collections
        .Concat(thisArgs)
        // Convert the result to a dictionary
        .ToDictionary(x => x.type, x => x.name);
    }

    /// <summary>
    /// Resolves generics for given <paramref name="source"/>
    /// </summary>
    /// <param name="source">Property to process</param>
    /// <param name="methods">Property methods</param>
    /// <returns>Resolved pairs for generic display names and raw names</returns>
    public static IReadOnlyDictionary<string, string> ResolvePropertyGenerics(this PropertyDef source, IReadOnlyCollection<MethodDef> methods)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      var outerArgs = GetGenericArguments(source.DeclaringType)
        // Distinct by name
        .DistinctBy(x => x.Name)
        // Pair generics with their display name and raw name
        .Select((x, i) => new { Type = x.Name, Name = $"`{i}" });

      var thisArgs = methods
        // Retrieve the generic parameters
        .Select(x => x.GenericParameters)
        // Flatten the collection
        .SelectMany(Linq.XtoX)
        // Distinct by name
        .DistinctBy(x => x.Name)
        // Pair generics with their display name and raw name
        .Select((x, i) => new { Type = x.Name, Name = $"``{i}" });

      return outerArgs
        // Join collections
        .Concat(thisArgs)
        // Convert the result to a dictionary
        .ToDictionary(x => x.Type.String, x => x.Name);
    }

    /// <summary>
    /// Resolves generics for given <paramref name="source"/>
    /// </summary>
    /// <param name="source">Type to process</param>
    /// <returns>Resolved pairs for generic display names and raw names</returns>
    public static IReadOnlyDictionary<string, string> ResolveTypeGenerics(this TypeDef source)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      // Retrieve generic arguments
      return GetGenericArguments(source.DeclaringType)
        // Distinct generics by their name
        .DistinctBy(x => x.Name)
        // Pair generics with their display name and raw name
        .Select((x, i) => new { Type = x.Name, Name = $"`{i}" })
        // Convert the result to a dictionary
        .ToDictionary(x => x.Type.String, x => x.Name);
    }

    private static IEnumerable<GenericParam> GetGenericArguments(TypeDef? type)
    {
      // If the type is null..
      if (type is null)
        // exit
        yield break;

      // Iterate over each generic argument in the parent type
      foreach (var parameter in GetGenericArguments(type.DeclaringType))
        // and return it
        yield return parameter;
      // Iterate over each generic parameter
      foreach (var parameter in type.GenericParameters)
        // and return it
        yield return parameter;
    }
  }
}
