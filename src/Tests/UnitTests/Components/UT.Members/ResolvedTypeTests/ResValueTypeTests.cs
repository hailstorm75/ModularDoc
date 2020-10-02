using System;
using System.Collections.Generic;
using System.Linq;
using MarkDoc.Members;
using MarkDoc.Members.Members;
using MarkDoc.Members.ResolvedTypes;
using MarkDoc.Members.Types;
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

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.RES_TYPES_NAMESPACE]
          .OfType<IClass>()
          .First(type => type.Name.Equals(Constants.RES_TYPE_CLASS));
        object?[] typeWrapper = {result};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetValueTypeNameData()
    {
      var data = new object[]
      {
        new object[] {Constants.METHOD_RES_BYTE, "byte"},
        new object[] {Constants.METHOD_RES_SBYTE, "sbyte"},
        new object[] {Constants.METHOD_RES_CHAR, "char"},
        new object[] {Constants.METHOD_RES_BOOL, "bool"},
        new object[] {Constants.METHOD_RES_STRING, "string"},
        new object[] {Constants.METHOD_RES_OBJECT, "object"},
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
        new object[] {Constants.METHOD_RES_REF_STRING, "string"},
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.RES_TYPES_NAMESPACE]
          .OfType<IClass>()
          .First(type => type.Name.Equals(Constants.RES_TYPE_CLASS));
        object?[] typeWrapper = {result};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    private static IMethod? GetMethod(IInterface type, string name, bool throwIfNull = false)
    {
      var member = type.Methods.FirstOrDefault(method => method.Name.Equals(name, StringComparison.InvariantCulture));

      if (throwIfNull && member is null)
        throw new KeyNotFoundException();

      return member;
    }

    #endregion

    [Theory]
    [Trait("Category", nameof(IResType))]
    [MemberData(nameof(GetValueTypeNameData))]
    public void ValidateValueTypeNames(IInterface type, string name, string returnType)
    {
      var member = GetMethod(type, name, true);

      Assert.Equal(returnType, member?.Returns?.DisplayName);
    }

    [Theory]
    [Trait("Category", nameof(IResType))]
    [MemberData(nameof(GetValueTypeRefData))]
    public void ValidateValueTypeIsRef(IInterface type, string name, bool isRef)
    {
      var member = GetMethod(type, name, true);

      Assert.Equal(isRef, member?.Returns?.IsByRef);
    }
  }
}