﻿using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;

namespace MarkDoc.Members.Dnlib.Helpers
{
  internal static class DynamicHelpers
  {
    private const string DYNAMIC_ATTRIBUTE_NAME = "System.Runtime.CompilerServices.DynamicAttribute";

    public static IReadOnlyList<bool>? GetDynamicTypes(this ParamDef parameter, TypeSig source)
    {
      // If the parameter is null..
      if (parameter is null)
        // throw an exception
        throw new ArgumentNullException(nameof(parameter));

      // Find the custom attribute indicating the dynamic type(s)
      var dynamicAttribute = parameter.CustomAttributes
        .FirstOrDefault(attribute => attribute.TypeFullName.Equals(DYNAMIC_ATTRIBUTE_NAME, StringComparison.InvariantCultureIgnoreCase));
      // Extract metadata from the custom attribute
      var arguments = (List<CAArgument>?)dynamicAttribute?.ConstructorArguments.FirstOrDefault().Value;
      // Extract the dynamic type indicators map
      var argument = arguments?
        // Select the indicator
        .Select(x => x.Value)
        // Skip the first one to remove the dummy indicator for the start of a generic type
        .Skip(1)
        // Cast the indicator values to booleans
        .Cast<bool>()
        // Materialize the collection
        .ToArray();

      // If there is no attribute..
      if (dynamicAttribute is null)
        // there are no dynamic types
        return null;

      // If there is no attribute metadata; however, there is an attribute..
      if (argument is null)
        // there is a single dynamic type
        return new[] { true };

      var index = 0;
      var map = new bool[argument.Length];

      void GenerateDummyMap(GenericInstSig token)
      {
        // For each generic arguments branch..
        foreach (var genericArgument in token.GenericArguments)
        {
          // Set the map with the node type - dummy = true = indicator for generic type
          map[index++] = genericArgument.IsGenericInstanceType;
          // If the node has children..
          if (genericArgument.IsGenericInstanceType)
            // Process its branches
            GenerateDummyMap(genericArgument.GetGenericSignature());
        }
      }

      GenerateDummyMap(source.GetGenericSignature());

      return argument
        // Pair indicators with their indices
        .Select((value, i) => (value, i))
        // Filter out the dummy indicators
        .Where(x => !map[x.i])
        // Extract the value
        .Select(x => x.value)
        // Materialize the collection
        .ToArray();
    }

    /// <summary>
    /// Counts the number of type arguments in each generic arguments branch
    /// </summary>
    /// <param name="source">Root source of generic arguments</param>
    /// <returns>List of the sums of arguments for each generic arguments branch</returns>
    public static IReadOnlyList<int> CountTypes(this GenericInstSig source)
    {
      static int GetTypes(TypeSig type)
        => type is GenericInstSig token
          ? token.GenericArguments.Sum(GetTypes)
          : 1;

      return source.GenericArguments
        .Select(GetTypes)
        // Materialize the collection
        .ToArray();
    }
  }
}