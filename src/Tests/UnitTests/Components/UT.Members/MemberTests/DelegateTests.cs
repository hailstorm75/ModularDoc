using System;
using System.Linq;
using System.Collections.Generic;
using MarkDoc.Helpers;
using UT.Members.Data;
using MarkDoc.Members.Members;
using MarkDoc.Members.Types;
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

      var data = new []
      {
        new object[] { Constants.DELEGATE_PUBLIC, FormatRawName($"{Constants.DELEGATE_PUBLIC}()") },
        new object[] { Constants.DELEGATE_PROTECTED, FormatRawName($"{Constants.DELEGATE_PROTECTED}()") },
        new object[] { Constants.DELEGATE_ARGUMENTS, FormatRawName($"{Constants.DELEGATE_ARGUMENTS}(System.String,System.String)") },
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IClass>(Constants.DELEGATES_NAMESPACE, Constants.DELEGATES_CLASS),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetDelegateNameData()
    {
      var filter = new HashSet<string> {Constants.DELEGATES_CLASS, Constants.DELEGATES_STRUCT, Constants.DELEGATES_INTERFACE};
      var data = new[]
      {
        new object[] {Constants.DELEGATE_PUBLIC}
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParents<IInterface>(Constants.DELEGATES_NAMESPACE, filter),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object?[]> GetDelegateAccessorData()
    {
      var data = new []
      {
        new object[] {Constants.DELEGATE_PUBLIC, AccessorType.Public},
        new object[] {Constants.DELEGATE_INTERNAL, AccessorType.Internal},
        new object[] {Constants.DELEGATE_PROTECTED, AccessorType.Protected},
        new object[] {Constants.DELEGATE_PROTECTED_INTERNAL, AccessorType.ProtectedInternal}
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IClass>(Constants.DELEGATES_NAMESPACE, Constants.DELEGATES_CLASS),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object?[]> GetDelegateData()
    {
      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(Constants.TEST_ASSEMBLY);

        var parents = resolver.FindMemberParents<IClass>(Constants.DELEGATES_NAMESPACE);
        foreach (var type in parents)
          foreach (var @delegate in type.Delegates)
            yield return new object[] {type, @delegate.Name};
      }
    }

    public static IEnumerable<object[]> GetDelegateGenericsData()
    {
      var data = new []
      {
        new object[] {Constants.DELEGATE_GENERIC, ("TDelegate", new string[] { })},
        new object[] {Constants.DELEGATE_GENERIC_CONSTRAINT, ("TDelegate", new[] {nameof(IDisposable)})}
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IClass>(Constants.DELEGATES_NAMESPACE, Constants.DELEGATES_CLASS_GENERIC),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetDelegateReturnData()
    {
      var data = new []
      {
        new object[] {Constants.DELEGATE_PUBLIC, null!},
        new object[] {Constants.DELEGATE_STRING, "string"}
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IInterface>(Constants.DELEGATES_NAMESPACE, Constants.DELEGATES_CLASS),
        Constants.TEST_ASSEMBLY);
    }

    private static IDelegate? GetDelegate(IInterface type, string name, bool throwIfNull = false)
      => type.Delegates.FindMember(name, throwIfNull);

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