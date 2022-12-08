using System;
using ModularDoc.Members.ResolvedTypes;
using ModularDoc.Members.Members;
using ModularDoc.Members.Types;
using ModularDoc.Helpers;
using System.Collections.Generic;
using System.Linq;
using UT.Members.Data;
using Xunit;

namespace UT.Members.ResolvedTypeTests
{
  public class ResValueTypeTests
  {
    #region Data providers

    public static IEnumerable<object[]> GetValueTypeRefData()
    {
      var data = new object[]
      {
        new object[] {Constants.METHOD_RES_BYTE, false},
        new object[] {Constants.METHOD_RES_SBYTE, false},
        new object[] {Constants.METHOD_RES_CHAR, false},
        new object[] {Constants.METHOD_RES_BOOL, false},
        new object[] {Constants.METHOD_RES_STRING, false},
        new object[] {Constants.METHOD_RES_OBJECT, false},
        new object[] {Constants.METHOD_RES_SHORT, false},
        new object[] {Constants.METHOD_RES_USHORT, false},
        new object[] {Constants.METHOD_RES_INT, false},
        new object[] {Constants.METHOD_RES_UINT, false},
        new object[] {Constants.METHOD_RES_LONG, false},
        new object[] {Constants.METHOD_RES_ULONG, false},
        new object[] {Constants.METHOD_RES_FLOAT, false},
        new object[] {Constants.METHOD_RES_DOUBLE, false},
        new object[] {Constants.METHOD_RES_DECIMAL, false},
        new object[] {Constants.METHOD_RES_DYNAMIC, false},
        new object[] {Constants.METHOD_RES_REF_STRING, true},
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetValueTypeReturnNameData()
    {
      var data = new object[]
      {
        new object[] {Constants.METHOD_RES_BYTE, "byte"},
        new object[] {Constants.METHOD_RES_SBYTE, "sbyte"},
        new object[] {Constants.METHOD_RES_CHAR, "char"},
        new object[] {Constants.METHOD_RES_BOOL, "bool"},
        new object[] {Constants.METHOD_RES_STRING, "string"},
        new object[] {Constants.METHOD_RES_OBJECT, "object"},
        new object[] {Constants.METHOD_RES_OBJR_MIXA, "object"},
        new object[] {Constants.METHOD_RES_OBJR_DYNA, "object"},
        new object[] {Constants.METHOD_RES_SHORT, "short"},
        new object[] {Constants.METHOD_RES_USHORT, "ushort"},
        new object[] {Constants.METHOD_RES_INT, "int"},
        new object[] {Constants.METHOD_RES_UINT, "uint"},
        new object[] {Constants.METHOD_RES_LONG, "long"},
        new object[] {Constants.METHOD_RES_ULONG, "ulong"},
        new object[] {Constants.METHOD_RES_FLOAT, "float"},
        new object[] {Constants.METHOD_RES_DOUBLE, "double"},
        new object[] {Constants.METHOD_RES_DECIMAL, "decimal"},
        new object[] {Constants.METHOD_RES_DYNAMIC, "dynamic"},
        new object[] {Constants.METHOD_RES_DYNR_MIXA, "dynamic"},
        new object[] {Constants.METHOD_RES_DYNR_OBJA, "dynamic"},
        new object[] {Constants.METHOD_RES_REF_STRING, "string"},
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetValueTypeArgumentNameData()
    {
      static IEnumerable<object[]> DataProvider(Func<string, IReadOnlyCollection<IArgument>> getter)
      {
        yield return new object[] { getter(Constants.METHOD_RES_DYNR_OBJA).ElementAt(0), "object" };
        yield return new object[] { getter(Constants.METHOD_RES_OBJR_DYNA).ElementAt(0), "dynamic" };
        yield return new object[] { getter(Constants.METHOD_RES_DYNR_MIXA).ElementAt(0), "object" };
        yield return new object[] { getter(Constants.METHOD_RES_OBJR_MIXA).ElementAt(0), "object" };
        yield return new object[] { getter(Constants.METHOD_RES_DYNR_MIXA).ElementAt(1), "dynamic" };
        yield return new object[] { getter(Constants.METHOD_RES_OBJR_MIXA).ElementAt(1), "dynamic" };
      }

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_CLASS);
        var data = DataProvider(x => parent.Methods.FindMember(x).Arguments).ToArray();
        foreach (object[] item in data)
          yield return item;
      }
    }

    public static IEnumerable<object[]> GetValueTypeData()
    {
      var data = new object[]
      {
        new object[] {Constants.METHOD_RES_BYTE},
        new object[] {Constants.METHOD_RES_SBYTE},
        new object[] {Constants.METHOD_RES_CHAR},
        new object[] {Constants.METHOD_RES_BOOL},
        new object[] {Constants.METHOD_RES_STRING},
        new object[] {Constants.METHOD_RES_OBJECT},
        new object[] {Constants.METHOD_RES_OBJR_MIXA},
        new object[] {Constants.METHOD_RES_OBJR_DYNA},
        new object[] {Constants.METHOD_RES_SHORT},
        new object[] {Constants.METHOD_RES_USHORT},
        new object[] {Constants.METHOD_RES_INT},
        new object[] {Constants.METHOD_RES_UINT},
        new object[] {Constants.METHOD_RES_LONG},
        new object[] {Constants.METHOD_RES_ULONG},
        new object[] {Constants.METHOD_RES_FLOAT},
        new object[] {Constants.METHOD_RES_DOUBLE},
        new object[] {Constants.METHOD_RES_DECIMAL},
        new object[] {Constants.METHOD_RES_DYNAMIC},
        new object[] {Constants.METHOD_RES_DYNR_MIXA},
        new object[] {Constants.METHOD_RES_DYNR_OBJA},
        new object[] {Constants.METHOD_RES_REF_STRING},
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetValueTypeRawNameData()
    {
      var data = new object[]
      {
        new object[] {Constants.METHOD_RES_BYTE, "System.Byte"},
        new object[] {Constants.METHOD_RES_SBYTE, "System.SByte"},
        new object[] {Constants.METHOD_RES_CHAR, "System.Char"},
        new object[] {Constants.METHOD_RES_BOOL, "System.Boolean"},
        new object[] {Constants.METHOD_RES_STRING, "System.String"},
        new object[] {Constants.METHOD_RES_OBJECT, "System.Object"},
        new object[] {Constants.METHOD_RES_OBJR_MIXA, "System.Object"},
        new object[] {Constants.METHOD_RES_OBJR_DYNA, "System.Object"},
        new object[] {Constants.METHOD_RES_SHORT, "System.Int16"},
        new object[] {Constants.METHOD_RES_USHORT, "System.UInt16"},
        new object[] {Constants.METHOD_RES_INT, "System.Int32"},
        new object[] {Constants.METHOD_RES_UINT, "System.UInt32"},
        new object[] {Constants.METHOD_RES_LONG, "System.Int64"},
        new object[] {Constants.METHOD_RES_ULONG, "System.UInt64"},
        new object[] {Constants.METHOD_RES_FLOAT, "System.Single"},
        new object[] {Constants.METHOD_RES_DOUBLE, "System.Double"},
        new object[] {Constants.METHOD_RES_DECIMAL, "System.Decimal"},
        new object[] {Constants.METHOD_RES_DYNAMIC, "System.Object"},
        new object[] {Constants.METHOD_RES_DYNR_MIXA, "System.Object"},
        new object[] {Constants.METHOD_RES_DYNR_OBJA, "System.Object"},
        new object[] {Constants.METHOD_RES_REF_STRING, "System.String"},
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_CLASS);
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
    [MemberData(nameof(GetValueTypeReturnNameData))]
    public void ValidateValueTypeReturnNames(IInterface type, string name, string returnType)
    {
      var member = GetMethod(type, name, true);

      Assert.Equal(returnType, member?.Returns?.DisplayName);
    }

    [Theory]
    [Trait("Category", nameof(IResType))]
    [MemberData(nameof(GetValueTypeRawNameData))]
    public void ValidateValueTypeDocumentationNames(IInterface type, string name, string rawName)
    {
      var member = GetMethod(type, name, true);

      Assert.Equal(rawName, member?.Returns?.DocumentationName);
    }

    [Theory]
    [Trait("Category", nameof(IResType))]
    [MemberData(nameof(GetValueTypeRawNameData))]
    public void ValidateValueTypeRawNames(IInterface type, string name, string rawName)
    {
      var member = GetMethod(type, name, true);

      Assert.Equal(rawName, member?.Returns?.RawName);
    }

    [Theory]
    [Trait("Category", nameof(IResType))]
    [MemberData(nameof(GetValueTypeRawNameData))]
    public void ValidateValueTypeNamespace(IInterface type, string name, string rawName)
    {
      var member = GetMethod(type, name, true);

      var typeNamespace = rawName.Remove(rawName.LastIndexOf('.'));
      Assert.Equal(typeNamespace, member?.Returns?.TypeNamespace);
    }

    [Theory]
    [Trait("Category", nameof(IResType))]
    [MemberData(nameof(GetValueTypeArgumentNameData))]
    public void ValidateValueTypeArgumentNames(IArgument argument, string returnType)
    {
      Assert.Equal(returnType, argument.Type.DisplayName);
    }

    [Theory]
    [Trait("Category", nameof(IResType))]
    [MemberData(nameof(GetValueTypeRefData))]
    public void ValidateValueTypeIsRef(IInterface type, string name, bool isRef)
    {
      var member = GetMethod(type, name, true);

      Assert.Equal(isRef, member?.Returns?.IsByRef);
    }

    [Theory]
    [Trait("Category", nameof(IResType))]
    [MemberData(nameof(GetValueTypeData))]
    public void ValidateValueTypeReference(IInterface type, string name)
    {
      var member = GetMethod(type, name, true);

      Assert.Null(member?.Returns?.Reference.Value);
    }
  }
}