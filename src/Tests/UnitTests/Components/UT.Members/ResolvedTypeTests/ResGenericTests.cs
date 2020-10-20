using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MarkDoc.Helpers;
using MarkDoc.Members.Members;
using MarkDoc.Members.ResolvedTypes;
using MarkDoc.Members.Types;
using UT.Members.Data;
using Xunit;

namespace UT.Members.ResolvedTypeTests
{
  public class ResGenericTests
  {
    #region Data providers

    public static IEnumerable<object[]> GetResGenericReturnParentNameData()
    {
      var data = new object[]
      {
        new object[] { Constants.METHOD_RES_GEN_ENUMSTRING, nameof(IEnumerable) },
        new object[] { Constants.METHOD_RES_GEN_DYNOBJ, "IReadOnlyDictionary" },
        new object[] { Constants.METHOD_RES_GEN_OBJDYN, "IReadOnlyDictionary" },
        new object[] { Constants.METHOD_RES_GEN_INTSTRBOOL, "Func" }
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_GENERIC_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetResGenericReturnGenericArgumentsData()
    {
      var data = new object[]
      {
        new object[] { Constants.METHOD_RES_GEN_ENUMSTRING, new [] { "string" } },
        new object[] { Constants.METHOD_RES_GEN_DYNOBJ, new [] { "dynamic", "object" } },
        new object[] { Constants.METHOD_RES_GEN_OBJDYN, new [] { "object", "dynamic" } },
        new object[] { Constants.METHOD_RES_GEN_INTSTRBOOL, new [] { "int", "string", "bool" } }
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_GENERIC_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetResGenericReturnComplexGenericArgumentsData()
    {
      var data = new object[]
      {
        new object[] { Constants.METHOD_RES_GEN_COMPLEX_A, new [] { "object", "object", "object", "object", "dynamic" } },
        new object[] { Constants.METHOD_RES_GEN_COMPLEX_B, new [] { "dynamic", "dynamic", "dynamic", "dynamic", "dynamic" } },
        new object[] { Constants.METHOD_RES_GEN_COMPLEX_C, new [] { "object", "dynamic", "object", "dynamic", "dynamic" } },
        new object[] { Constants.METHOD_RES_GEN_COMPLEX_D, new [] { "dynamic", "object", "dynamic", "object", "dynamic" } },
        new object[] { Constants.METHOD_RES_GEN_COMPLEX_E, new [] { "object", "object", "object", "object", "object" } },
        new object[] { Constants.METHOD_RES_GEN_COMPLEX_F, new [] { "dynamic", "dynamic", "dynamic", "dynamic", "object" } },
        new object[] { Constants.METHOD_RES_GEN_COMPLEX_G, new [] { "object", "dynamic", "object", "dynamic", "object" } },
        new object[] { Constants.METHOD_RES_GEN_COMPLEX_H, new [] { "dynamic", "object", "dynamic", "object", "object" } },
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_GENERIC_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    private static IMethod? GetMethod(IInterface type, string name, bool throwIfNull = false)
      => type.Methods.FindMember(name, throwIfNull);

    #endregion

    [Theory]
    [Trait("Category", nameof(IResGeneric))]
    [MemberData(nameof(GetResGenericReturnGenericArgumentsData))]
    public void ValidateGenericsCount(IInterface type, string name, string[] generics)
    {
      var member = GetMethod(type, name, true);
      var returns = (IResGeneric?)member?.Returns;

      Assert.Equal(generics.Length, returns?.Generics.Count);
    }

    [Theory]
    [Trait("Category", nameof(IResGeneric))]
    [MemberData(nameof(GetResGenericReturnComplexGenericArgumentsData))]
    public void ValidateGenericsComplexReturnGenericParameters(IInterface type, string name, string[] generics)
    {
      var member = GetMethod(type, name, true);
      var returns = (IResGeneric?)member?.Returns;

      var types = returns?.Generics.OfType<IResGeneric>().ToArray();
      var a = types?.First()
        .Generics
        .Select(x => x.DisplayName)
        .Concat(types?.Last().Generics.OfType<IResGeneric>().First().Generics.Select(x => x.DisplayName) ?? Enumerable.Empty<string>())
        .Concat(new[] { types?.Last().Generics.OfType<IResGeneric>().Last().Generics.Last().DisplayName })
        .ToArray();

      Assert.Equal(generics, a);
    }

    [Theory]
    [Trait("Category", nameof(IResGeneric))]
    [MemberData(nameof(GetResGenericReturnGenericArgumentsData))]
    public void ValidateGenericsNames(IInterface type, string name, string[] generics)
    {
      var member = GetMethod(type, name, true);
      var returns = (IResGeneric?)member?.Returns;

      Assert.Equal(generics, returns?.Generics.Select(x => x.DisplayName));
    }

    [Theory]
    [Trait("Category", nameof(IResGeneric))]
    [MemberData(nameof(GetResGenericReturnParentNameData))]
    public void ValidateGenericsParentNames(IInterface type, string name, string parent)
    {
      var member = GetMethod(type, name, true);
      var returns = (IResGeneric?)member?.Returns;

      Assert.Equal(parent, returns?.DisplayName);
    }
  }
}