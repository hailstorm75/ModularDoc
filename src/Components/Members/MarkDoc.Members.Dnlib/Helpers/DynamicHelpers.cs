using System;
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
      if (parameter is null)
        throw new ArgumentNullException(nameof(parameter));

      var dynamicAttribute = parameter.CustomAttributes.FirstOrDefault(attribute => attribute.TypeFullName.Equals(DYNAMIC_ATTRIBUTE_NAME, StringComparison.InvariantCultureIgnoreCase));
      var arguments = (List<CAArgument>?)dynamicAttribute?.ConstructorArguments.FirstOrDefault().Value;
      var argument = arguments?.Select(x => x.Value).Skip(1).Cast<bool>().ToArray();

      if (argument is null
          && dynamicAttribute != null)
        return new[] { true };

      if (argument is null)
        return null;

      var index = 0;
      bool[] map = new bool[argument.Length];

      void Process(GenericInstSig token)
      {
        foreach (var genericArgument in token.GenericArguments)
          if (map[index++] = genericArgument.IsGenericInstanceType)
            Process(genericArgument.GetGenericSignature());
      }

      Process(source.GetGenericSignature());

      return argument
        .Select((x, i) => (x, i))
        .Where(x => !map[x.i])
        .Select(x => x.x).ToArray();
    }

    public static int[] CountTypes(this GenericInstSig source)
    {
      int GetTypes(TypeSig type)
      {
        if (!(type is GenericInstSig token))
          return 1;

        return token.GenericArguments.Sum(GetTypes);
      }

      return source.GenericArguments.Select(GetTypes).ToArray();
    }
  }
}
