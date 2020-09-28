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

    public static IEnumerable<object[]> GetDelegateNameData()
    {
      var filter = new HashSet<string> { Constants.DELEGATES_CLASS, Constants.DELEGATES_STRUCT, Constants.DELEGATES_INTERFACE };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var types = resolver?.Types.Value[Constants.DELEGATES_NAMESPACE]
          .OfType<IInterface>()
          .Where(type => filter.Contains(type.Name));
        foreach (var type in types ?? throw new Exception())
          yield return new object[] { type, Constants.DELEGATE_PUBLIC };
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

        var result = resolver?.Types.Value[Constants.DELEGATES_NAMESPACE].OfType<IClass>().First(type => type.Name.Equals(Constants.DELEGATES_CLASS));
        object?[] typeWrapper = { result };
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray();
      }
    }

    public static IEnumerable<object[]> GetDelegateGenericsData()
    {
      var data = new object[]
      {
        new object[] {Constants.DELEGATE_GENERIC, ("TDelegate", new string[] { })},
        new object[] {Constants.DELEGATE_GENERIC_CONSTRAINT, ("TDelegate", new [] {nameof(IDisposable)})}
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
    [MemberData(nameof(GetDelegateNameData))]
    public void ValidateDelegateNames(IInterface type, string name)
    {
      var member = GetDelegate(type, name);

      Assert.NotNull(member);
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
    public void ValidateDelegateGenericNames(IInterface type, string name, (string generic, string[] constraints) generics)
    {
      var members = GetDelegate(type, name, true);
      var generic = members?.Generics.First();

      Assert.Equal(generics.generic, generic?.Key);
    }

    [Theory]
    [Trait("Category", nameof(IDelegate))]
    [MemberData(nameof(GetDelegateGenericsData))]
    public void ValidateDelegateGenericConstraints(IInterface type, string name, (string generic, string[] constraints) generics)
    {
      var members = GetDelegate(type, name, true);
      var generic = members?.Generics.First();

      Assert.Equal(generics.constraints, generic?.Value.Select(c => c.DisplayName));
    }
  }
}
