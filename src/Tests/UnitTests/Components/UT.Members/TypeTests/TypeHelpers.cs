using System.Collections.Generic;
using System.Linq;
using MarkDoc.Helpers;
using MarkDoc.Members;
using MarkDoc.Members.Types;
using UT.Members.Data;

namespace UT.Members.TypeTests
{
  internal static class TypeHelpers
  {
    public static IReadOnlyCollection<T> GetTypes<T>(this IResolver resolver)
      where T : IType
      => resolver.Types.Value
        .SelectMany(types => types.Value)
        .OfType<T>()
        .ToReadOnlyCollection();

    public static IEnumerable<object[]> ComposeData(this IEnumerable<IReadOnlyList<object>> dataCollection)
    {
      foreach (var data in dataCollection)
      {
        foreach (var resolver in new ResolversProvider())
        {
          var collection = new object[1 + data.Count];
          collection[0] = resolver.First();
          for (var i = 1; i <= data.Count; i++)
            collection[i] = data[i - 1];

          yield return collection;
        }
      }
    }

    public static IEnumerable<object?[]> ComposeData(this IEnumerable<object?> dataCollection)
    {
      foreach (var data in dataCollection)
        foreach (var resolver in new ResolversProvider())
        {
          var collection = new object?[2];
          collection[0] = resolver.First();
          collection[1] = data;

          yield return collection;
        }
    }
  }
}
