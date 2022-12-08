using System.Collections.Generic;
using System.Linq;
using ModularDoc.Helpers;
using ModularDoc.Members.Members;
using ModularDoc.Members.ResolvedTypes;
using ModularDoc.Members.Types;
using UT.Members.Data;
using Xunit;

namespace UT.Members.ResolvedTypeTests
{
  public class ResGenericValueTypeTests
  {
    #region Data provider

    public static IEnumerable<object[]> GetGenericValueTypeNames()
    {
      var data = new object[]
      {
        new object[] {Constants.METHOD_RES_GENERIC_VALUE_TYPES, new[] { "T1", "T2" }},
        new object[] {Constants.METHOD_RES_GENERIC_OWN_VALUE_TYPES, new[] { "TA", "TB" }},
        new object[] {Constants.METHOD_RES_GENERIC_MIXED_VALUE_TYPES, new[] { "T1", "TA", "T2", "TB" }},
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_GENERIC_VALUE_TYPE_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    private static IMethod? GetMethod(IInterface type, string name, bool throwIfNull = false)
      => type.Methods.FindMember(name, throwIfNull);

    #endregion

    [Theory]
    [Trait("Category", nameof(IResType))]
    [MemberData(nameof(GetGenericValueTypeNames))]
    public void ValidateValueTypeIsRef(IInterface type, string name, string[] names)
    {
      var member = GetMethod(type, name, true);

      Assert.Equal(names, member?.Arguments.Select(argument => argument.Type.DisplayName).ToArray());
    }
  }
}