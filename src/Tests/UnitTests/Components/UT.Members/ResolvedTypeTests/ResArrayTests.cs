using MarkDoc.Members.ResolvedTypes;
using MarkDoc.Members.Members;
using MarkDoc.Members.Types;
using MarkDoc.Helpers;
using System.Collections.Generic;
using System.Linq;
using UT.Members.Data;
using Xunit;

namespace UT.Members.ResolvedTypeTests
{
  public class ResArrayTests
  {
    #region Data providers

    public static IEnumerable<object[]> GetArrayReturnReferenceData()
    {
      var data = new object[]
      {
        new object[] { Constants.METHOD_RES_1D_ARRAY, null! },
        new object[] { Constants.METHOD_RES_2D_ARRAY, null! },
        new object[] { Constants.METHOD_RES_3D_ARRAY, null! },
        new object[] { Constants.METHOD_RES_2D_JAGGED_ARRAY, null! },
        new object[] { Constants.METHOD_RES_3D_JAGGED_ARRAY, null! },
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_ARRAY_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetArrayReturnRefData()
    {
      var data = new object[]
      {
        new object[] { Constants.METHOD_RES_1D_ARRAY, false },
        new object[] { Constants.METHOD_RES_2D_ARRAY, false },
        new object[] { Constants.METHOD_RES_3D_ARRAY, false },
        new object[] { Constants.METHOD_RES_2D_JAGGED_ARRAY, false },
        new object[] { Constants.METHOD_RES_3D_JAGGED_ARRAY, false },
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_ARRAY_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetArrayReturnDisplayNameData()
    {
      var data = new object[]
      {
        new object[] { Constants.METHOD_RES_1D_ARRAY, "string" },
        new object[] { Constants.METHOD_RES_2D_ARRAY, "string" },
        new object[] { Constants.METHOD_RES_3D_ARRAY, "string" },
        new object[] { Constants.METHOD_RES_2D_JAGGED_ARRAY, "string" },
        new object[] { Constants.METHOD_RES_3D_JAGGED_ARRAY, "string" }
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_ARRAY_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetArrayReturnDocumentationNameData()
    {
      var data = new object[]
      {
        new object[] { Constants.METHOD_RES_1D_ARRAY, "System.String[]" },
        new object[] { Constants.METHOD_RES_2D_ARRAY, "System.String[,]" },
        new object[] { Constants.METHOD_RES_3D_ARRAY, "System.String[,,]" },
        new object[] { Constants.METHOD_RES_2D_JAGGED_ARRAY, "System.String[][]" },
        new object[] { Constants.METHOD_RES_3D_JAGGED_ARRAY, "System.String[][][]" }
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_ARRAY_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetArrayReturnRawNameData()
    {
      var data = new object[]
      {
        new object[] { Constants.METHOD_RES_1D_ARRAY, "System.String[]" },
        new object[] { Constants.METHOD_RES_2D_ARRAY, "System.String[0...,0...]" },
        new object[] { Constants.METHOD_RES_3D_ARRAY, "System.String[0...,0...,0...]" },
        new object[] { Constants.METHOD_RES_2D_JAGGED_ARRAY, "System.String[][]" },
        new object[] { Constants.METHOD_RES_3D_JAGGED_ARRAY, "System.String[][][]" }
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_ARRAY_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    private static IMethod? GetMethod(IInterface type, string name, bool throwIfNull = false)
      => type.Methods.FindMember(name, throwIfNull);

    #endregion

    [Theory]
    [Trait("Category", nameof(IResArray))]
    [MemberData(nameof(GetArrayReturnReferenceData))]
    public void ValidateArrayReference(IInterface type, string name, IType? reference)
    {
      var member = GetMethod(type, name, true);

      Assert.Equal(reference, member?.Returns?.Reference.Value);
    }

    [Theory]
    [Trait("Category", nameof(IResArray))]
    [MemberData(nameof(GetArrayReturnRefData))]
    public void ValidateArrayIsRef(IInterface type, string name, bool isRef)
    {
      var member = GetMethod(type, name, true);

      Assert.Equal(isRef, member?.Returns?.IsByRef);
    }

    [Theory]
    [Trait("Category", nameof(IResArray))]
    [MemberData(nameof(GetArrayReturnDisplayNameData))]
    public void ValidateArrayReturnDisplayNames(IInterface type, string name, string returnType)
    {
      var member = GetMethod(type, name, true);

      Assert.Equal(returnType, member?.Returns?.DisplayName);
    }

    [Theory]
    [Trait("Category", nameof(IResArray))]
    [MemberData(nameof(GetArrayReturnRawNameData))]
    public void ValidateArrayReturnRawNames(IInterface type, string name, string returnType)
    {
      var member = GetMethod(type, name, true);

      Assert.Equal(returnType, member?.Returns?.RawName);
    }

    [Theory]
    [Trait("Category", nameof(IResArray))]
    [MemberData(nameof(GetArrayReturnDocumentationNameData))]
    public void ValidateArrayDocumentationNames(IInterface type, string name, string returnType)
    {
      var member = GetMethod(type, name, true);

      Assert.Equal(returnType, member?.Returns?.DocumentationName);
    }

    [Theory]
    [Trait("Category", nameof(IResArray))]
    [MemberData(nameof(GetArrayReturnDocumentationNameData))]
    public void ValidateArrayNamespace(IInterface type, string name, string rawName)
    {
      var member = GetMethod(type, name, true);

      var typeNamespace = rawName.Remove(rawName.LastIndexOf('.'));
      Assert.Equal(typeNamespace, member?.Returns?.TypeNamespace);
    }
  }
}
