using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;

namespace MarkDoc.Members.Dnlib.Helpers
{
  internal static class MetadataHelpers
  {
    #region Fields

    private const string DYNAMIC_ATTRIBUTE_NAME = "System.Runtime.CompilerServices.DynamicAttribute";
    private const string TUPLES_NAMES_ATTRIBUTE_NAME = "System.Runtime.CompilerServices.TupleElementNamesAttribute";

    #endregion

    public static IReadOnlyList<string>? GetValueTupleNames(this ParamDef parameter)
    {
      // If the parameter is null..
      if (parameter is null)
        // throw an exception
        throw new ArgumentNullException(nameof(parameter));

      // Find the custom attribute indicating the dynamic type(s)
      var customAttribute = parameter.CustomAttributes
        .FirstOrDefault(attribute =>
          attribute.TypeFullName.Equals(TUPLES_NAMES_ATTRIBUTE_NAME, StringComparison.InvariantCultureIgnoreCase));

      // If there is no attribute..
      if (customAttribute is null)
        // there are no dynamic types
        return null;

      // Extract metadata from the custom attribute
      var arguments = (List<CAArgument>?) customAttribute.ConstructorArguments.FirstOrDefault().Value;
      // Extract the dynamic type indicators map
      var argument = arguments?
        // Select the indicator
        .Select(x => x.Value?.ToString() ?? string.Empty)
        // Materialize the collection
        .ToArray();

      return argument;
    }

    public static IReadOnlyList<bool>? GetDynamicTypes(this ParamDef parameter, TypeSig source)
    {
      // If the parameter is null..
      if (parameter is null)
        // throw an exception
        throw new ArgumentNullException(nameof(parameter));

      // Find the custom attribute indicating the dynamic type(s)
      var dynamicAttribute = parameter.CustomAttributes
        .FirstOrDefault(attribute =>
          attribute.TypeFullName.Equals(DYNAMIC_ATTRIBUTE_NAME, StringComparison.InvariantCultureIgnoreCase));

      // If there is no attribute..
      if (dynamicAttribute is null)
        // there are no dynamic types
        return null;

      // Extract metadata from the custom attribute
      var arguments = (List<CAArgument>?) dynamicAttribute.ConstructorArguments.FirstOrDefault().Value;
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

      // If there is no attribute metadata; however, there is an attribute..
      if (argument is null)
        // there is a single dynamic type
        return new[] {true};

      var index = 0;
      var map = new bool[argument.Length];

      void GenerateDummyMap(TypeSig token)
      {
        var arraySkip = 0;
        // Extract the generic type from the array and count the array depth
        var genericSource = token.ExtractIfArray(ref arraySkip).GetGenericSignature();
        // For every level deep the type was inside the array..
        for (var i = 0; i < arraySkip; ++i)
          // set the map to ignore given position
          map![index++] = true;

        // For each generic arguments branch..
        foreach (var genericArgument in genericSource.GenericArguments)
        {
          // If the type is generic..
          if (genericArgument.IsGenericInstanceType)
            // ignore the identifier
            map![index++] = true;
          // Otherwise if the type is an array..
          else if (genericArgument.ElementType == ElementType.Array || genericArgument.ElementType == ElementType.SZArray)
          {
            arraySkip = 0;
            // Extract the generic type from the array and count the array depth
            genericArgument.ExtractIfArray(ref arraySkip);
            // For every level deep the type was inside the array..
            for (var i = 0; i < arraySkip; ++i)
              // set the map to ignore given position
              map![index++] = true;
          }
          // Otherwise..
          else
            // increase the index of the identifier position
            index++;

          // If the node has children..
          if (genericArgument.IsGenericInstanceType)
            // Process its branches
            GenerateDummyMap(genericArgument.GetGenericSignature());
        }
      }

      GenerateDummyMap(source);

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
  }
}