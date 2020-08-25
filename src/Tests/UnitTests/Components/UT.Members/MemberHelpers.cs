using MarkDoc.Members;
using System.Collections.Generic;
using System.Linq;
using MarkDoc.Helpers;
using MarkDoc.Members.Types;

namespace UT.Members
{
  internal static class MemberHelpers
  {
    public static IReadOnlyCollection<T> GetTypes<T>(this IResolver resolver)
      where T : IType
      => resolver.Types.Value
          .SelectMany(types => types.Value)
          .OfType<T>()
          .ToReadOnlyCollection();
  }
}
