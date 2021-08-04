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
  public class ResTupleTests
  {
    #region Data provider

    public static IEnumerable<object[]> GetTupleParameterTypeData()
    {
      var data = new object[]
      {
        new object[] {Constants.METHOD_RES_TUPLE_ONE, new[] { "short" }},
        new object[] {Constants.METHOD_RES_TUPLE_TWO, new[] { "short", "int" }},
        new object[] {Constants.METHOD_RES_TUPLE_THREE, new[] { "short", "int", "long" }},
        new object[] {Constants.METHOD_RES_TUPLE_FOUR, new[] { "short", "int", "long", "byte" }},
        new object[] {Constants.METHOD_RES_VALUE_TUPLE_TWO, new[] { "short", "int" }},
        new object[] {Constants.METHOD_RES_VALUE_TUPLE_THREE, new[] { "short", "int", "long" }},
        new object[] {Constants.METHOD_RES_VALUE_TUPLE_FOUR, new[] { "short", "int", "long", "byte" }},
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_TUPLE_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetTupleParameterNameData()
    {
      var data = new object[]
      {
        new object[] {Constants.METHOD_RES_TUPLE_ONE, new[] { "Item1" }},
        new object[] {Constants.METHOD_RES_TUPLE_TWO, new[] { "Item1", "Item2" }},
        new object[] {Constants.METHOD_RES_TUPLE_THREE, new[] { "Item1", "Item2", "Item3" }},
        new object[] {Constants.METHOD_RES_TUPLE_FOUR, new[] { "Item1", "Item2", "Item3", "Item4" }},
        new object[] {Constants.METHOD_RES_VALUE_TUPLE_TWO, new[] { "a", "b" }},
        new object[] {Constants.METHOD_RES_VALUE_TUPLE_THREE, new[] { "a", "b", "c" }},
        new object[] {Constants.METHOD_RES_VALUE_TUPLE_FOUR, new[] { "a", "b", "c", "d" }},
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_TUPLE_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetTupleParameterCountData()
    {
      var data = new object[]
      {
        new object[] { Constants.METHOD_RES_TUPLE_ONE, 1 },
        new object[] { Constants.METHOD_RES_TUPLE_TWO, 2 },
        new object[] { Constants.METHOD_RES_TUPLE_THREE, 3 },
        new object[] { Constants.METHOD_RES_TUPLE_FOUR, 4 },
        new object[] { Constants.METHOD_RES_VALUE_TUPLE_TWO, 2 },
        new object[] { Constants.METHOD_RES_VALUE_TUPLE_THREE, 3 },
        new object[] { Constants.METHOD_RES_VALUE_TUPLE_FOUR, 4 },
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_TUPLE_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetIsValueTupleData()
    {
      var data = new object[]
      {
        new object[] { Constants.METHOD_RES_TUPLE_ONE, false },
        new object[] { Constants.METHOD_RES_TUPLE_TWO, false },
        new object[] { Constants.METHOD_RES_TUPLE_THREE, false },
        new object[] { Constants.METHOD_RES_TUPLE_FOUR, false },
        new object[] { Constants.METHOD_RES_TUPLE_COMPLEX, false },
        new object[] { Constants.METHOD_RES_VALUE_TUPLE_TWO, true },
        new object[] { Constants.METHOD_RES_VALUE_TUPLE_THREE, true },
        new object[] { Constants.METHOD_RES_VALUE_TUPLE_FOUR, true },
        new object[] { Constants.METHOD_RES_VALUE_TUPLE_COMPLEX, true },
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_TUPLE_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetComplexTupleTypeData()
    {
      var data = new object[]
      {
        new object[] { Constants.METHOD_RES_TUPLE_COMPLEX, new[] { "object", "Tuple<dynamic,object>", "dynamic", "object", "dynamic" } },
        new object[] { Constants.METHOD_RES_VALUE_TUPLE_COMPLEX, new[] { "object", "(dynamic a2, object b2)", "dynamic", "object", "dynamic" } },
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_TUPLE_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetComplexTupleNameData()
    {
      var data = new object[]
      {
        new object[] { Constants.METHOD_RES_TUPLE_COMPLEX, new[] { "Item1", "Item2", "Item1", "Item2", "Item3" } },
        new object[] { Constants.METHOD_RES_VALUE_TUPLE_COMPLEX, new[] { "a1", "b1", "a2", "b2", "c1" } },
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.RES_TYPES_NAMESPACE, Constants.RES_TYPE_TUPLE_CLASS);
        object?[] typeWrapper = {parent};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    private static IMethod? GetMethod(IInterface type, string name, bool throwIfNull = false)
      => type.Methods.FindMember(name, throwIfNull);

    #endregion

    [Theory]
    [Trait("Category", nameof(IResTuple))]
    [MemberData(nameof(GetTupleParameterTypeData))]
    public void ValidateTupleParameterTypes(IInterface type, string name, string[] types)
    {
      var member = GetMethod(type, name, true);

      var returns = (IResTuple?)member?.Returns;

      Assert.Equal(types, returns?.Fields.Select(field => field.type.DisplayName));
    }

    [Theory]
    [Trait("Category", nameof(IResTuple))]
    [MemberData(nameof(GetTupleParameterNameData))]
    public void ValidateTupleParameterNames(IInterface type, string name, string[] names)
    {
      var member = GetMethod(type, name, true);

      var returns = (IResTuple?)member?.Returns;

      Assert.Equal(names, returns?.Fields.Select(field => field.name));
    }

    [Theory]
    [Trait("Category", nameof(IResTuple))]
    [MemberData(nameof(GetTupleParameterCountData))]
    public void ValidateTupleParameterCount(IInterface type, string name, int count)
    {
      var member = GetMethod(type, name, true);

      var returns = (IResTuple?)member?.Returns;

      Assert.Equal(count, returns?.Fields.Count);
    }

    [Theory]
    [Trait("Category", nameof(IResTuple))]
    [MemberData(nameof(GetIsValueTupleData))]
    public void ValidateIsValueTuple(IInterface type, string name, bool isValueTuple)
    {
      var member = GetMethod(type, name, true);

      var returns = (IResTuple?)member?.Returns;

      Assert.Equal(isValueTuple, returns?.IsValueTuple);
    }

    [Theory]
    [Trait("Category", nameof(IResTuple))]
    [MemberData(nameof(GetComplexTupleTypeData))]
    public void ValidateComplexTupleParameterTypes(IInterface type, string name, string[] names)
    {
      var member = GetMethod(type, name, true);
      var returns = (IResTuple?)member?.Returns;

      var parameters = returns?.Fields
        .Take(2)
        .Concat(((IResTuple) returns.Fields.ElementAt(1).type).Fields)
        .Concat(returns.Fields.Skip(2));

      Assert.Equal(names, parameters?.Select(parameter => parameter.type.DisplayName));
    }

    [Theory]
    [Trait("Category", nameof(IResTuple))]
    [MemberData(nameof(GetComplexTupleNameData))]
    public void ValidateComplexTupleParameterNames(IInterface type, string name, string[] names)
    {
      var member = GetMethod(type, name, true);
      var returns = (IResTuple?)member?.Returns;

      var parameters = returns?.Fields
        .Take(2)
        .Concat(((IResTuple) returns.Fields.ElementAt(1).type).Fields)
        .Concat(returns.Fields.Skip(2));

      Assert.Equal(names, parameters?.Select(parameter => parameter.name));
    }
  }
}