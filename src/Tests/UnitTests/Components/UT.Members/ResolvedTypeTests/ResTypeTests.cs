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
  public class ResTypeTests
  {
    #region Data providers

    public static IEnumerable<object[]> GetTypeRawNameData()
    {
      var data = new object[]
      {
        new object[] { Constants.METHOD_RES_NESTED_GENERIC_RET, "TestLibrary.Classes.ClassParent.NestedGenericClass{System.Int32,System.IO.StreamReader}" },
        new object[] { Constants.METHOD_RES_NESTED_RET, "TestLibrary.Classes.ClassParent.NestedClassPublic" },
        new object[] { Constants.METHOD_RES_NESTED_GENERIC_VALUE_TYPES_RET, "TestLibrary.Classes.ClassParent.NestedGenericClass{`0,`1}" },
        new object[] { Constants.METHOD_RES_NESTED_GENERIC_PARENT_RET, "TestLibrary.ResTypes.ClassResTypes.NestedResTypesGeneric{`0}.NestedNestedResTypesGeneric" },
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPES_CLASS);
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
    [MemberData(nameof(GetTypeRawNameData))]
    public void ValidateValueTypeDocumentationNames(IInterface type, string name, string rawName)
    {
      var member = GetMethod(type, name, true);

      Assert.Equal(rawName, member?.Returns?.DocumentationName);
    }
  }
}