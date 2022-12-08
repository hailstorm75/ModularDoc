using ModularDoc.Members.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using ModularDoc.Helpers;
using ModularDoc.Members.Enums;
using ModularDoc.Members.Types;
using UT.Members.Data;
using Xunit;

namespace UT.Members.MemberTests
{
  public class MethodTests
  {
    #region Data providers

    private static IEnumerable<(string name, OperatorType type)> GetOperators()
      => new[]
      {
        (Constants.METHOD_ADDITION, OperatorType.Normal),
        (Constants.METHOD_SUBSTRACTION, OperatorType.Normal),
        (Constants.METHOD_MULTIPLY, OperatorType.Normal),
        (Constants.METHOD_DIVISION, OperatorType.Normal),
        (Constants.METHOD_MODULUS, OperatorType.Normal),
        (Constants.METHOD_EXCLUSIVEOR, OperatorType.Normal),
        (Constants.METHOD_BITWISEAND, OperatorType.Normal),
        (Constants.METHOD_BITWISEOR, OperatorType.Normal),
        (Constants.METHOD_LOGICALNOT, OperatorType.Normal),
        (Constants.METHOD_LEFTSHIFT, OperatorType.Normal),
        (Constants.METHOD_RIGHTSHIFT, OperatorType.Normal),
        (Constants.METHOD_EQUALITY, OperatorType.Normal),
        (Constants.METHOD_INEQUALITY, OperatorType.Normal),
        (Constants.METHOD_GREATERTHAN, OperatorType.Normal),
        (Constants.METHOD_LESSTHAN, OperatorType.Normal),
        (Constants.METHOD_GREATERTHANEQUALS, OperatorType.Normal),
        (Constants.METHOD_LESSTHANEQUALS, OperatorType.Normal),
        (Constants.METHOD_DECREMENT, OperatorType.Normal),
        (Constants.METHOD_INCREMENT, OperatorType.Normal),
        (Constants.METHOD_ONESCOMPLEMENT, OperatorType.Normal),
        (Constants.METHOD_IMPLICIT, OperatorType.Implicit),
        (Constants.METHOD_EXPLICIT, OperatorType.Explicit)
      };

    public static IEnumerable<object[]> GetMethodAccessorData()
    {
      var data = new []
      {
        new object[] {Constants.METHOD_PUBLIC, AccessorType.Public},
        new object[] {Constants.METHOD_INTERNAL, AccessorType.Internal},
        new object[] {Constants.METHOD_PROTECTED, AccessorType.Protected},
        new object[] {Constants.METHOD_PROTECTED_INTERNAL, AccessorType.ProtectedInternal}
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IClass>(Constants.METHODS_NAMESPACE, Constants.METHODS_CLASS),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetMethodNameData()
    {
      var operators = GetOperators().ToArray();
      var filter = new HashSet<string> {Constants.METHODS_CLASS, Constants.METHODS_STRUCT, Constants.METHODS_INTERFACE};

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParents<IInterface>(Constants.METHODS_NAMESPACE, filter).ToArray();
        foreach (var type in parent)
          yield return new object[] {type, Constants.METHOD_PUBLIC};

        var classMethod =
          parent.First(type => type.Name.Equals(Constants.METHODS_CLASS, StringComparison.InvariantCulture));
        foreach (var @operator in operators)
          yield return new object[] {classMethod, @operator.name};
      }
    }

    public static IEnumerable<object[]> GetMethodRawNameData()
    {
      static string FormatRawName(string methodName)
        => string.Format($"{Constants.METHODS_NAMESPACE}.{Constants.METHODS_CLASS}.{{0}}", methodName);

      var data = new []
      {
        new object[] {Constants.METHOD_PUBLIC, FormatRawName($"{Constants.METHOD_PUBLIC}()")},
        new object[]
        {
          Constants.METHOD_ADDITION,
          FormatRawName(
            "op_Addition(TestLibrary.Members.Methods.ClassMethods,TestLibrary.Members.Methods.ClassMethods)")
        },
        new object[]
        {
          Constants.METHOD_SUBSTRACTION,
          FormatRawName(
            "op_Subtraction(TestLibrary.Members.Methods.ClassMethods,TestLibrary.Members.Methods.ClassMethods)")
        },
        new object[]
        {
          Constants.METHOD_MULTIPLY,
          FormatRawName(
            "op_Multiply(TestLibrary.Members.Methods.ClassMethods,TestLibrary.Members.Methods.ClassMethods)")
        },
        new object[]
        {
          Constants.METHOD_DIVISION,
          FormatRawName(
            "op_Division(TestLibrary.Members.Methods.ClassMethods,TestLibrary.Members.Methods.ClassMethods)")
        },
        new object[]
        {
          Constants.METHOD_MODULUS,
          FormatRawName("op_Modulus(TestLibrary.Members.Methods.ClassMethods,TestLibrary.Members.Methods.ClassMethods)")
        },
        new object[]
        {
          Constants.METHOD_EXCLUSIVEOR,
          FormatRawName(
            "op_Exclusiveor(TestLibrary.Members.Methods.ClassMethods,TestLibrary.Members.Methods.ClassMethods)")
        },
        new object[]
        {
          Constants.METHOD_BITWISEAND,
          FormatRawName(
            "op_Bitwiseand(TestLibrary.Members.Methods.ClassMethods,TestLibrary.Members.Methods.ClassMethods)")
        },
        new object[]
        {
          Constants.METHOD_BITWISEOR,
          FormatRawName(
            "op_Bitwiseor(TestLibrary.Members.Methods.ClassMethods,TestLibrary.Members.Methods.ClassMethods)")
        },
        new object[]
          {Constants.METHOD_LOGICALNOT, FormatRawName("op_Logicalnot(TestLibrary.Members.Methods.ClassMethods)")},
        new object[]
        {
          Constants.METHOD_LEFTSHIFT,
          FormatRawName("op_Leftshift(TestLibrary.Members.Methods.ClassMethods,System.Int32)")
        },
        new object[]
        {
          Constants.METHOD_RIGHTSHIFT,
          FormatRawName("op_Rightshift(TestLibrary.Members.Methods.ClassMethods,System.Int32)")
        },
        new object[]
        {
          Constants.METHOD_EQUALITY,
          FormatRawName(
            "op_Equality(TestLibrary.Members.Methods.ClassMethods,TestLibrary.Members.Methods.ClassMethods)")
        },
        new object[]
        {
          Constants.METHOD_INEQUALITY,
          FormatRawName(
            "op_Inequality(TestLibrary.Members.Methods.ClassMethods,TestLibrary.Members.Methods.ClassMethods)")
        },
        new object[]
        {
          Constants.METHOD_GREATERTHAN,
          FormatRawName(
            "op_Greaterthan(TestLibrary.Members.Methods.ClassMethods,TestLibrary.Members.Methods.ClassMethods)")
        },
        new object[]
        {
          Constants.METHOD_LESSTHAN,
          FormatRawName(
            "op_Lessthan(TestLibrary.Members.Methods.ClassMethods,TestLibrary.Members.Methods.ClassMethods)")
        },
        new object[]
        {
          Constants.METHOD_GREATERTHANEQUALS,
          FormatRawName(
            "op_Greaterthanorequal(TestLibrary.Members.Methods.ClassMethods,TestLibrary.Members.Methods.ClassMethods)")
        },
        new object[]
        {
          Constants.METHOD_LESSTHANEQUALS,
          FormatRawName(
            "op_Lessthanorequal(TestLibrary.Members.Methods.ClassMethods,TestLibrary.Members.Methods.ClassMethods)")
        },
        new object[]
          {Constants.METHOD_DECREMENT, FormatRawName("op_Decrement(TestLibrary.Members.Methods.ClassMethods)")},
        new object[]
          {Constants.METHOD_INCREMENT, FormatRawName("op_Increment(TestLibrary.Members.Methods.ClassMethods)")},
        new object[]
        {
          Constants.METHOD_ONESCOMPLEMENT, FormatRawName("op_Onescomplement(TestLibrary.Members.Methods.ClassMethods)")
        },
        new object[]
          {Constants.METHOD_IMPLICIT, FormatRawName("op_Implicit(TestLibrary.Members.Methods.ClassMethods)")},
        new object[]
          {Constants.METHOD_EXPLICIT, FormatRawName("op_Explicit(TestLibrary.Members.Methods.ClassMethods)")},
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IClass>(Constants.METHODS_NAMESPACE, Constants.METHODS_CLASS),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetMethodInheritanceData()
    {
      var data = new[]
      {
        new object[] {Constants.METHOD_NORMAL, MemberInheritance.Normal},
        new object[] {Constants.METHOD_OVERRIDE, MemberInheritance.Override},
        new object[] {Constants.METHOD_ABSTRACT, MemberInheritance.Abstract},
        new object[] {Constants.METHOD_VIRTUAL, MemberInheritance.Virtual},
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IClass>(Constants.METHODS_NAMESPACE, Constants.METHODS_CLASS_ABSTRACT),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetMethodIsStaticData()
    {
      var data = new []
      {
        new object[] {Constants.METHOD_PUBLIC, false},
        new object[] {Constants.METHOD_STATIC, true},
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IClass>(Constants.METHODS_NAMESPACE, Constants.METHODS_CLASS),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetMethodIsAsyncData()
    {
      var data = new[]
      {
        new object[] {Constants.METHOD_PUBLIC, false},
        new object[] {Constants.METHOD_ASYNC, true},
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IClass>(Constants.METHODS_NAMESPACE, Constants.METHODS_CLASS),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetMethodGenericsData()
    {
      var data = new[]
      {
        new object[] {Constants.METHOD_GENERIC, ("TMethod", new string[] { })},
        new object[] {Constants.METHOD_GENERIC_CONSTRAINTS, ("TMethod", new[] {nameof(IDisposable)})}
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IClass>(Constants.METHODS_NAMESPACE, Constants.METHODS_CLASS_GENERIC),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetMethodReturnData()
    {
      var data = new[]
      {
        new object[] {Constants.METHOD_PUBLIC, null!},
        new object[] {Constants.METHOD_STRING, "string"},
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IClass>(Constants.METHODS_NAMESPACE, Constants.METHODS_CLASS),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetMethodOperatorData()
    {
      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parent = resolver.FindMemberParent<IClass>(Constants.METHODS_NAMESPACE, Constants.METHODS_CLASS);
        foreach (var (name, type) in GetOperators())
          yield return parent.WrapItem().Concat(new object[] {name, type}).ToArray();
      }
    }

    private static IMethod? GetMethod(IInterface type, string name, bool throwIfNull = false)
      => type.Methods.FindMember(name, throwIfNull);

    #endregion

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetMethodReturnData))]
    public void ValidateMethodReturnTypes(IInterface type, string name, string? returnType)
    {
      var member = GetMethod(type, name, true);

      Assert.Equal(returnType, member?.Returns?.DisplayName);
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetMethodNameData))]
    public void ValidateMethodNames(IInterface type, string name)
    {
      var member = GetMethod(type, name);

      Assert.NotNull(member);
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetMethodRawNameData))]
    public void ValidateMethodRawNames(IInterface type, string name, string rawName)
    {
      var member = GetMethod(type, name);

      Assert.Equal(rawName.ToLowerInvariant(), member?.RawName.ToLowerInvariant());
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetMethodAccessorData))]
    public void ValidateMethodAccessors(IInterface type, string name, AccessorType methods)
    {
      var member = GetMethod(type, name);

      Assert.Equal(methods, member?.Accessor);
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetMethodInheritanceData))]
    public void ValidateMethodInheritance(IInterface type, string name, MemberInheritance inheritance)
    {
      var members = GetMethod(type, name, true);

      Assert.Equal(inheritance, members?.Inheritance);
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetMethodIsStaticData))]
    public void ValidateMethodIsStatic(IInterface type, string name, bool isStatic)
    {
      var members = GetMethod(type, name, true);

      Assert.Equal(isStatic, members?.IsStatic);
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetMethodIsAsyncData))]
    public void ValidateMethodIsAsync(IInterface type, string name, bool isAsync)
    {
      var members = GetMethod(type, name, true);

      Assert.Equal(isAsync, members?.IsAsync);
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetMethodOperatorData))]
    public void ValidateMethodOperators(IInterface type, string name, OperatorType operatorType)
    {
      var members = GetMethod(type, name, true);

      Assert.Equal(operatorType, members?.Operator);
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetMethodGenericsData))]
    public void ValidateMethodGenericNames(IInterface type, string name,
      (string generic, string[] constraints) generics)
    {
      var members = GetMethod(type, name, true);
      var generic = members?.Generics.First();

      Assert.Equal(generics.generic, generic?.Key);
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetMethodGenericsData))]
    public void ValidateMethodGenericConstraints(IInterface type, string name,
      (string generic, string[] constraints) generics)
    {
      var members = GetMethod(type, name, true);
      var generic = members?.Generics.First();

      Assert.Equal(generics.constraints, generic?.Value.Select(c => c.DisplayName));
    }
  }
}