using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using MarkDoc.Helpers;

namespace MarkDoc.Members.Dnlib.Helpers
{
  internal static class TupleHelpers
  {
    private const string TUPLES_NAMES_ATTRIBUTE_NAME = "System.Runtime.CompilerServices.TupleElementNamesAttribute";

    public static IReadOnlyList<string>? GetValueTupleNames(this ParamDef parameter, TypeSig source)
    {
      // If the parameter is null..
      if (parameter is null)
        // throw an exception
        throw new ArgumentNullException(nameof(parameter));

      // Find the custom attribute indicating the dynamic type(s)
      var customAttribute = parameter.CustomAttributes
        .FirstOrDefault(attribute => attribute.TypeFullName.Equals(TUPLES_NAMES_ATTRIBUTE_NAME, StringComparison.InvariantCultureIgnoreCase));
      // Extract metadata from the custom attribute
      var arguments = (List<CAArgument>?)customAttribute?.ConstructorArguments.FirstOrDefault().Value;
      // Extract the dynamic type indicators map
      var argument = arguments?
        // Select the indicator
        .Select(x => x.Value?.ToString())
        // Cast the indicator values to booleans
        .WhereNotNull()
        // Materialize the collection
        .ToArray();

      // If there is no attribute..
      if (customAttribute is null)
        // there are no dynamic types
        return null;

      return argument;
    }
  }
}
