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

    // public static IEnumerable<object[]> GetMethodGenericsData()
    // {
    // }

    // public static IEnumerable<object[]> GetMethodReturnData()
    // {
    // }

    // public static IEnumerable<object[]> GetMethodOperatorData()
    // {
    // }

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
    [MemberData(nameof(GetMethodNameData))]
    public void ValidatePropertyNames(IInterface type, string name)
    {
      var member = GetMethod(type, name);

      Assert.NotNull(member);
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetMethodAccessorData))]
    public void ValidatePropertyAccessors(IInterface type, string name, AccessorType methods)
    {
      var member = GetMethod(type, name);

      Assert.Equal(methods, member?.Accessor);
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetMethodInheritanceData))]
    public void ValidatePropertyInheritance(IInterface type, string name, MemberInheritance inheritance)
    {
      var members = GetMethod(type, name, true);

      Assert.Equal(inheritance, members?.Inheritance);
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetMethodIsStaticData))]
    public void ValidatePropertyIsStatic(IInterface type, string name, bool isStatic)
    {
      var members = GetMethod(type, name, true);

      Assert.Equal(isStatic, members?.IsStatic);
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetMethodIsAsyncData))]
    public void ValidatePropertyIsAsync(IInterface type, string name, bool isAsync)
    {
      var members = GetMethod(type, name, true);

      Assert.Equal(isAsync, members?.IsAsync);
    }
  }
}