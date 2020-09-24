using MarkDoc.Members.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using MarkDoc.Members;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Types;
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

    public static IEnumerable<object?[]> GetMethodAccessorData()
    {
      var data = new object[]
      {
        new object[] {Constants.METHOD_PUBLIC, AccessorType.Public},
        new object[] {Constants.METHOD_INTERNAL, AccessorType.Internal},
        new object[] {Constants.METHOD_PROTECTED, AccessorType.Protected},
        new object[] {Constants.METHOD_PROTECTED_INTERNAL, AccessorType.ProtectedInternal}
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.METHODS_NAMESPACE]
          .OfType<IClass>()
          .First(type => type.Name.Equals(Constants.METHODS_CLASS));
        object?[] typeWrapper = {result};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray();
      }
    }

    public static IEnumerable<object[]> GetMethodNameData()
    {
      var operators = GetOperators().ToArray();
      var filter = new HashSet<string> {Constants.METHODS_CLASS, Constants.METHODS_STRUCT, Constants.METHODS_INTERFACE};

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var types = resolver?.Types.Value[Constants.METHODS_NAMESPACE]
          .OfType<IInterface>()
          .Where(type => filter.Contains(type.Name));
        foreach (var type in types ?? throw new Exception())
          yield return new object[] {type, Constants.METHOD_PUBLIC};

        var classMethod =
          types.First(type => type.Name.Equals(Constants.METHODS_CLASS, StringComparison.InvariantCulture));
        foreach (var @operator in operators)
          yield return new object[] {classMethod, @operator.name};
      }
    }

    // public static IEnumerable<object[]> GetMethodRawNameData()
    // {
    // }

    public static IEnumerable<object[]> GetMethodInheritanceData()
    {
      var data = new object[]
      {
        new object[] {Constants.METHOD_NORMAL, MemberInheritance.Normal},
        new object[] {Constants.METHOD_OVERRIDE, MemberInheritance.Override},
        new object[] {Constants.METHOD_ABSTRACT, MemberInheritance.Abstract},
        new object[] {Constants.METHOD_VIRTUAL, MemberInheritance.Virtual},
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.METHODS_NAMESPACE]
          .OfType<IClass>()
          .First(type => type.Name.Equals(Constants.METHODS_CLASS_ABSTRACT));
        object?[] typeWrapper = {result};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetMethodIsStaticData()
    {
      var data = new object[]
      {
        new object[] {Constants.METHOD_PUBLIC, false},
        new object[] {Constants.METHOD_STATIC, true},
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.METHODS_NAMESPACE]
          .OfType<IClass>()
          .First(type => type.Name.Equals(Constants.METHODS_CLASS));
        object?[] typeWrapper = {result};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetMethodIsAsyncData()
    {
      var data = new object[]
      {
        new object[] {Constants.METHOD_PUBLIC, false},
        new object[] {Constants.METHOD_ASYNC, true},
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.METHODS_NAMESPACE]
          .OfType<IClass>()
          .First(type => type.Name.Equals(Constants.METHODS_CLASS));
        object?[] typeWrapper = {result};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetMethodGenericsData()
    {
      var data = new object[]
      {
        new object[] {Constants.METHOD_GENERIC, ("TMethod", new string[] { })},
        new object[] {Constants.METHOD_GENERIC_CONSTRAINTS, ("TMethod", new [] {nameof(IDisposable)})}
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.METHODS_NAMESPACE]
          .OfType<IClass>()
          .First(type => type.Name.Equals(Constants.METHODS_CLASS_GENERIC));
        object?[] typeWrapper = {result};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetMethodReturnData()
    {
      var data = new object[]
      {
        new object?[] {Constants.METHOD_PUBLIC, null!},
        new object?[] {Constants.METHOD_STRING, "string"},
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.METHODS_NAMESPACE]
          .OfType<IClass>()
          .First(type => type.Name.Equals(Constants.METHODS_CLASS));
        object?[] typeWrapper = {result};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetMethodOperatorData()
    {
      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.METHODS_NAMESPACE]
          .OfType<IClass>()
          .First(type => type.Name.Equals(Constants.METHODS_CLASS));
        object?[] typeWrapper = {result};
        foreach (var (name, type) in GetOperators())
          yield return typeWrapper.Concat(new object[] { name, type }).ToArray()!;
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
    public void ValidateMethodGenericNames(IInterface type, string name, (string generic, string[] constraints) generics)
    {
      var members = GetMethod(type, name, true);
      var generic = members?.Generics.First();

      Assert.Equal(generics.generic, generic?.Key);
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetMethodGenericsData))]
    public void ValidateMethodGenericConstraints(IInterface type, string name, (string generic, string[] constraints) generics)
    {
      var members = GetMethod(type, name, true);
      var generic = members?.Generics.First();

      Assert.Equal(generics.constraints, generic?.Value.Select(c => c.DisplayName));
    }
  }
}