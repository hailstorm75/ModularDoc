using System;
using System.Linq;
using System.Collections.Generic;
using UT.Members.Data;
using MarkDoc.Members.Members;
using MarkDoc.Members.Types;
using MarkDoc.Members;
using Xunit;
using MarkDoc.Members.Enums;

namespace UT.Members.MemberTests
{
  public class DelegateTests
  {
    #region Data provider

    public static IEnumerable<object?[]> GetDelegateRawNameData()
    {
      static string FormatRawName(string methodName)
        => string.Format($"{Constants.DELEGATES_NAMESPACE}.{Constants.DELEGATES_CLASS}.{{0}}", methodName);

      var data = new object[]
      {
        new object[] { Constants.DELEGATE_PUBLIC, FormatRawName($"{Constants.DELEGATE_PUBLIC}()") },
        new object[] { Constants.DELEGATE_PROTECTED, FormatRawName($"{Constants.DELEGATE_PROTECTED}()") },
        new object[] { Constants.DELEGATE_ARGUMENTS, FormatRawName($"{Constants.DELEGATE_ARGUMENTS}(System.String,System.String)") },
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.DELEGATES_NAMESPACE]
          .OfType<IClass>()
          .First(type => type.Name.Equals(Constants.DELEGATES_CLASS));
        object?[] typeWrapper = {result};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray();
      }
    }

    public static IEnumerable<object[]> GetDelegateNameData()
    {
      var filter = new HashSet<string>
        {Constants.DELEGATES_CLASS, Constants.DELEGATES_STRUCT, Constants.DELEGATES_INTERFACE};

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var types = resolver?.Types.Value[Constants.DELEGATES_NAMESPACE]
          .OfType<IInterface>()
          .Where(type => filter.Contains(type.Name));
        foreach (var type in types ?? throw new Exception())
          yield return new object[] {type, Constants.DELEGATE_PUBLIC};
      }
    }

    public static IEnumerable<object?[]> GetDelegateAccessorData()
    {
      var data = new object[]
      {
        new object[] {Constants.DELEGATE_PUBLIC, AccessorType.Public},
        new object[] {Constants.DELEGATE_INTERNAL, AccessorType.Internal},
        new object[] {Constants.DELEGATE_PROTECTED, AccessorType.Protected},
        new object[] {Constants.DELEGATE_PROTECTED_INTERNAL, AccessorType.ProtectedInternal}
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.DELEGATES_NAMESPACE]
          .OfType<IClass>()
          .First(type => type.Name.Equals(Constants.DELEGATES_CLASS));
        object?[] typeWrapper = {result};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray();
      }
    }

    public static IEnumerable<object?[]> GetDelegateData()
    {
      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.DELEGATES_NAMESPACE].OfType<IClass>();
        foreach (var type in result ?? throw new Exception())
          foreach (var @delegate in type.Delegates)
            yield return new object[] {type, @delegate.Name};
      }
    }

    public static IEnumerable<object[]> GetDelegateGenericsData()
    {
      var data = new object[]
      {
        new object[] {Constants.DELEGATE_GENERIC, ("TDelegate", new string[] { })},
        new object[] {Constants.DELEGATE_GENERIC_CONSTRAINT, ("TDelegate", new[] {nameof(IDisposable)})}
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.DELEGATES_NAMESPACE]
          .OfType<IClass>()
          .First(type => type.Name.Equals(Constants.DELEGATES_CLASS_GENERIC));
        object?[] typeWrapper = {result};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetDelegateReturnData()
    {
      var data = new object[]
      {
        new object?[] {Constants.DELEGATE_PUBLIC, null!},
        new object[] {Constants.DELEGATE_STRING, "string"}
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.DELEGATES_NAMESPACE]
          .OfType<IClass>()
          .First(type => type.Name.Equals(Constants.DELEGATES_CLASS));
        object?[] typeWrapper = {result};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    private static IDelegate? GetDelegate(IInterface type, string name, bool throwIfNull = false)
    {
      var member = type.Delegates.FirstOrDefault(method => method.Name.Equals(name, StringComparison.InvariantCulture));

      if (throwIfNull && member is null)
        throw new KeyNotFoundException();

      return member;
    }

    #endregion

    [Theory]
    [Trait("Category", nameof(IDelegate))]
    [MemberData(nameof(GetDelegateData))]
    public void ValidateDelegateIsStatic(IInterface type, string name)
    {
      var member = GetDelegate(type, name, true);

      Assert.False(member?.IsStatic);
    }

    [Theory]
    [Trait("Category", nameof(IDelegate))]
    [MemberData(nameof(GetDelegateNameData))]
    public void ValidateDelegateNames(IInterface type, string name)
    {
      var member = GetDelegate(type, name);

      Assert.NotNull(member);
    }

    [Theory]
    [Trait("Category", nameof(IDelegate))]
    [MemberData(nameof(GetDelegateRawNameData))]
    public void ValidateDelegateRawNames(IInterface type, string name, string rawName)
    {
      var member = GetDelegate(type, name);

      Assert.Equal(rawName.ToLowerInvariant(), member?.RawName.ToLowerInvariant());
    }

    [Theory]
    [Trait("Category", nameof(IDelegate))]
    [MemberData(nameof(GetDelegateAccessorData))]
    public void ValidateDelegateAccessors(IInterface type, string name, AccessorType delegateAccessor)
    {
      var member = GetDelegate(type, name);

      Assert.Equal(delegateAccessor, member?.Accessor);
    }

    [Theory]
    [Trait("Category", nameof(IDelegate))]
    [MemberData(nameof(GetDelegateGenericsData))]
    public void ValidateDelegateGenericNames(IInterface type, string name,
      (string generic, string[] constraints) generics)
    {
      var members = GetDelegate(type, name, true);
      var generic = members?.Generics.First();

      Assert.Equal(generics.generic, generic?.Key);
    }

    [Theory]
    [Trait("Category", nameof(IDelegate))]
    [MemberData(nameof(GetDelegateGenericsData))]
    public void ValidateDelegateGenericConstraints(IInterface type, string name,
      (string generic, string[] constraints) generics)
    {
      var members = GetDelegate(type, name, true);
      var generic = members?.Generics.First();

      Assert.Equal(generics.constraints, generic?.Value.Select(c => c.DisplayName));
    }

    [Theory]
    [Trait("Category", nameof(IDelegate))]
    [MemberData(nameof(GetDelegateReturnData))]
    public void ValidateDelegateReturns(IInterface type, string name, string returns)
    {
      var members = GetDelegate(type, name, true);

      Assert.Equal(returns, members?.Returns?.DisplayName);
    }
  }
}