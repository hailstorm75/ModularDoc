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
        new object[] {Constants.METHOD_RES_1D_ARRAY, null!},
        new object[] {Constants.METHOD_RES_2D_ARRAY, null!},
        new object[] {Constants.METHOD_RES_3D_ARRAY, null!},
        new object[] {Constants.METHOD_RES_2D_JAGGED_ARRAY, null!},
        new object[] {Constants.METHOD_RES_3D_JAGGED_ARRAY, null!},
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
        new object[] {Constants.METHOD_RES_1D_ARRAY, false},
        new object[] {Constants.METHOD_RES_2D_ARRAY, false},
        new object[] {Constants.METHOD_RES_3D_ARRAY, false},
        new object[] {Constants.METHOD_RES_2D_JAGGED_ARRAY, false},
        new object[] {Constants.METHOD_RES_3D_JAGGED_ARRAY, false},
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
        new object[] {Constants.METHOD_RES_1D_ARRAY, "string"},
        new object[] {Constants.METHOD_RES_2D_ARRAY, "string"},
        new object[] {Constants.METHOD_RES_3D_ARRAY, "string"},
        new object[] {Constants.METHOD_RES_2D_JAGGED_ARRAY, "string"},
        new object[] {Constants.METHOD_RES_3D_JAGGED_ARRAY, "string"}
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
        new object[] {Constants.METHOD_RES_1D_ARRAY, "System.String[]"},
        new object[] {Constants.METHOD_RES_2D_ARRAY, "System.String[,]"},
        new object[] {Constants.METHOD_RES_3D_ARRAY, "System.String[,,]"},
        new object[] {Constants.METHOD_RES_2D_JAGGED_ARRAY, "System.String[][]"},
        new object[] {Constants.METHOD_RES_3D_JAGGED_ARRAY, "System.String[][][]"}
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
        new object[] {Constants.METHOD_RES_1D_ARRAY, "System.String[]"},
        new object[] {Constants.METHOD_RES_2D_ARRAY, "System.String[0...,0...]"},
        new object[] {Constants.METHOD_RES_3D_ARRAY, "System.String[0...,0...,0...]"},
        new object[] {Constants.METHOD_RES_2D_JAGGED_ARRAY, "System.String[][]"},
        new object[] {Constants.METHOD_RES_3D_JAGGED_ARRAY, "System.String[][][]"}
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

    public static IEnumerable<object[]> GetResTupleArrayReturnComplexGenericArgumentsData()
    {
      var data = new object[]
      {
        new object[] {Constants.METHOD_RES_ARR_TUPLE_COMPLEX, new[] {"object", "dynamic", "object", "dynamic"}},
        new object[] {Constants.METHOD_RES_ARR_VALUE_TUPLE_COMPLEX, new[] {"object", "dynamic", "object", "dynamic"}},
        new object[] {Constants.METHOD_RES_ARR_TUPLE_COMPLEX2, new[] {"object", "dynamic", "object", "dynamic"}},
        new object[] {Constants.METHOD_RES_ARR_VALUE_TUPLE_COMPLEX2, new[] {"object", "dynamic", "object", "dynamic"}},
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

    public static IEnumerable<object[]> GetResGenericArrayReturnGenericArgumentsData()
    {
      var data = new object[]
      {
        new object[] {Constants.METHOD_RES_GEN_ARR_ENUMSTRING, new[] {"string"}},
        new object[] {Constants.METHOD_RES_GEN_ARR_DYNOBJ, new[] {"dynamic", "object"}},
        new object[] {Constants.METHOD_RES_GEN_ARR_OBJDYN, new[] {"object", "dynamic"}},
        new object[] {Constants.METHOD_RES_GEN_ARR_INTSTRBOOL, new[] {"int", "string", "bool"}}
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

    public static IEnumerable<object[]> GetResGenericArrayReturnComplexGenericArgumentsData()
    {
      var data = new object[]
      {
        new object[]
          {Constants.METHOD_RES_GEN_ARR_COMPLEX_A, new[] {"object", "object", "object", "object", "dynamic"}},
        new object[]
          {Constants.METHOD_RES_GEN_ARR_COMPLEX_B, new[] {"dynamic", "dynamic", "dynamic", "dynamic", "dynamic"}},
        new object[]
          {Constants.METHOD_RES_GEN_ARR_COMPLEX_C, new[] {"object", "dynamic", "object", "dynamic", "dynamic"}},
        new object[]
          {Constants.METHOD_RES_GEN_ARR_COMPLEX_D, new[] {"dynamic", "object", "dynamic", "object", "dynamic"}},
        new object[] {Constants.METHOD_RES_GEN_ARR_COMPLEX_E, new[] {"object", "object", "object", "object", "object"}},
        new object[]
          {Constants.METHOD_RES_GEN_ARR_COMPLEX_F, new[] {"dynamic", "dynamic", "dynamic", "dynamic", "object"}},
        new object[]
          {Constants.METHOD_RES_GEN_ARR_COMPLEX_G, new[] {"object", "dynamic", "object", "dynamic", "object"}},
        new object[]
          {Constants.METHOD_RES_GEN_ARR_COMPLEX_H, new[] {"dynamic", "object", "dynamic", "object", "object"}},
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

    private static IEnumerable<string> ExtractGenericTypeNames(IEnumerable<IResType> types)
    {
      var actualGenerics = new LinkedList<string>();
      void Process(IResType resType)
      {
        switch (resType)
        {
          case IResArray arr:
            Process(arr.ArrayType);
            break;
          case IResTuple tup:
            foreach (var field in tup.Fields.Select(x => x.type))
              Process(field!);
            break;
          case IResGeneric gen:
            foreach (var field in gen.Generics)
              Process(field!);
            break;
          default:
            actualGenerics!.AddLast(resType!.DisplayName);
            break;
        }
      }

      foreach (var type in types)
        Process(type);

      return actualGenerics;
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

    [Theory]
    [Trait("Category", nameof(IResArray))]
    [Trait("Category", nameof(IResTuple))]
    [MemberData(nameof(GetResTupleArrayReturnComplexGenericArgumentsData))]
    public void ValidateTupleArrayComplexReturnGenericParameters(IInterface type, string name, string[] generics)
    {
      var member = GetMethod(type, name, true);
      var returns = (IResArray) member?.Returns!;
      var wrappedType = (IResTuple) returns!.ArrayType!;

      var actualGenerics = ExtractGenericTypeNames(wrappedType.Fields.Select(x => x.type));

      Assert.Equal(generics, actualGenerics);
    }

    [Theory]
    [Trait("Category", nameof(IResArray))]
    [Trait("Category", nameof(IResGeneric))]
    [MemberData(nameof(GetResGenericArrayReturnGenericArgumentsData))]
    public void ValidateGenericArrayNames(IInterface type, string name, string[] generics)
    {
      var member = GetMethod(type, name, true);
      var returns = (IResArray) member?.Returns!;
      var wrappedType = (IResGeneric) returns!.ArrayType!;

      Assert.Equal(generics, wrappedType!.Generics.Select(x => x.DisplayName));
    }

    [Theory]
    [Trait("Category", nameof(IResArray))]
    [Trait("Category", nameof(IResGeneric))]
    [MemberData(nameof(GetResGenericArrayReturnComplexGenericArgumentsData))]
    public void ValidateGenericsArrayComplexReturnGenericParameters(IInterface type, string name, string[] generics)
    {
      var member = GetMethod(type, name, true);
      var returns = (IResArray) member?.Returns!;
      var wrappedType = (IResGeneric) returns!.ArrayType!;

      var actualGenerics = ExtractGenericTypeNames(wrappedType.Generics.OfType<IResGeneric>());

      Assert.Equal(generics, actualGenerics);
    }
  }
}