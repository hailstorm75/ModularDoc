using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;

namespace MarkDoc.Members.Dnlib.Helpers
{
  public static class DynamicHelpers
  {
    private const string DYNAMIC_ATTRIBUTE_NAME = "System.Runtime.CompilerServices.DynamicAttribute";

    public static IReadOnlyList<bool>? GetDynamicTypes(this ParamDef parameter)
    {
      if (parameter is null)
        throw new ArgumentNullException(nameof(parameter));

      var dynamicAttribute = parameter.CustomAttributes.FirstOrDefault(attribute => attribute.TypeFullName.Equals(DYNAMIC_ATTRIBUTE_NAME, StringComparison.InvariantCultureIgnoreCase));
      var arguments = (List<CAArgument>?)dynamicAttribute?.ConstructorArguments.FirstOrDefault().Value;
      var argument = arguments?.Select(x => x.Value).Cast<bool>().Skip(1).ToArray();

      if (argument is null
          && dynamicAttribute != null)
        return new[] {true};

      return argument;
    }
  }
}
