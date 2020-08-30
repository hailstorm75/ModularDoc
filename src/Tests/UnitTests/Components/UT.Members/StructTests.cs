using System.Collections.Generic;
using System.Linq;
using MarkDoc.Members.Types;
using MarkDoc.Members.Enums;
using MarkDoc.Members;
using MarkDoc.Members.Members;
using UT.Members.Data;
using Xunit;

namespace UT.Members
{
  public class StructTests
  {
    #region Data providers

    private static IEnumerable<object[]> GetStructNames()
    {
      var data = new[]
      {
        Constants.PUBLIC_STRUCT,
        Constants.INTERNAL_STRUCT,
        Constants.PUBLIC_NESTED_STRUCT,
        Constants.PROTECTED_NESTED_STRUCT,
        Constants.INTERNAL_NESTED_STRUCT,
      };

      foreach (var name in data)
        foreach (var resolver in new ResolversProvider())
          yield return new[] { resolver.First(), name };
    }

    private static IEnumerable<object[]> GetStructAccessorsData()
    {
      var data = new[]
      {
        (Constants.PUBLIC_STRUCT, AccessorType.Public),
        (Constants.INTERNAL_STRUCT, AccessorType.Internal),
        (Constants.PUBLIC_NESTED_STRUCT, AccessorType.Public),
        (Constants.PROTECTED_NESTED_STRUCT, AccessorType.Protected),
        (Constants.INTERNAL_NESTED_STRUCT, AccessorType.Internal),
      };

      foreach (var (name, accessor) in data)
        foreach (var resolver in new ResolversProvider())
          yield return new[] { resolver.First(), name, accessor };
    }

    private static IEnumerable<object[]> GetStructNamespaceData()
    {
      const string interfaceNameSpace = "TestLibrary.Structs";
      var data = new[]
      {
        (Constants.PUBLIC_STRUCT, interfaceNameSpace),
        (Constants.INTERNAL_STRUCT, interfaceNameSpace),
        (Constants.PUBLIC_NESTED_STRUCT, $"{interfaceNameSpace}.StructParent"),
        (Constants.PROTECTED_NESTED_STRUCT, $"{interfaceNameSpace}.StructParent"),
        (Constants.INTERNAL_NESTED_STRUCT, $"{interfaceNameSpace}.StructParent"),
      };

      foreach (var (name, space) in data)
        foreach (var resolver in new ResolversProvider())
          yield return new[] { resolver.First(), name, space };
    }

    private static IEnumerable<object[]> GetStructWithMembers()
      => new ResolversProvider().Select(resolver => new[] { resolver.First(), Constants.PUBLIC_STRUCT });

    private static IEnumerable<object[]> GetStructMemberData(string member)
      => new ResolversProvider().Select(resolver => new[] { resolver.First(), Constants.PUBLIC_STRUCT, member });

    private static IEnumerable<object[]> GetStructPropertiesData()
      => GetStructMemberData("Property");

    private static IEnumerable<object[]> GetStructMethodsData()
      => GetStructMemberData("Method");

    private static IEnumerable<object[]> GetStructEventsData()
      => GetStructMemberData("Event");

    private static IEnumerable<object[]> GetStructDelegatesData()
      => GetStructMemberData("Delegate");

    #endregion

    private static IStruct? GetStruct(IResolver resolver, string name)
    {
      resolver.Resolve(Constants.TEST_ASSEMBLY);

      return resolver
        .GetTypes<IStruct>()
        .FirstOrDefault(type => type.Name.Equals(name));
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [MemberData(nameof(GetStructNames))]
    public void ValidateStructNames(IResolver resolver, string name)
    {
      var query = GetStruct(resolver, name);

      Assert.NotNull(query);
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [MemberData(nameof(GetStructNamespaceData))]
    public void ValidateStructRawName(IResolver resolver, string name, string expectedNamespace)
    {
      var query = GetStruct(resolver, name);

      Assert.True(query?.RawName.Equals($"{expectedNamespace}.{name}"), $"{resolver.GetType().FullName}: The '{name}' raw name is invalid. Expected '{expectedNamespace}.{name}' != Actual '{query?.RawName}'.");
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [MemberData(nameof(GetStructAccessorsData))]
    public void ValidateStructAccessors(IResolver resolver, string name, AccessorType accessor)
    {
      var query = GetStruct(resolver, name);

      Assert.True(query?.Accessor == accessor, $"{resolver.GetType().FullName}: The '{name}' accessor type is invalid. Expected '{accessor}' != Actual '{query?.Accessor}'");
    }

    [Theory]
    [Trait("Category", nameof(IEnum))]
    [MemberData(nameof(GetStructNamespaceData))]
    public void ValidateStructNamespace(IResolver resolver, string name, string expectedNamespace)
    {
      var query = GetStruct(resolver, name);

      Assert.True(query?.TypeNamespace.Equals(expectedNamespace), $"{resolver.GetType().FullName}: The '{name}' namespace is invalid. Expected '{expectedNamespace}' != Actual '{query?.TypeNamespace}'.");
    }

    [Theory]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetStructPropertiesData))]
    public void ValidateStructProperties(IResolver resolver, string name, string member)
    {
      var query = GetStruct(resolver, name);

      var hasProperty = query?.Properties.Any(property => property.Name.Equals(member)) ?? false;

      Assert.True(hasProperty, $"{resolver.GetType().FullName}: The '{name}' struct is missing the '{member}'.");
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetStructMethodsData))]
    public void ValidateStructMethods(IResolver resolver, string name, string member)
    {
      var query = GetStruct(resolver, name);

      var hasMethod = query?.Methods.Any(method => method.Name.Equals(member)) ?? false;

      Assert.True(hasMethod, $"{resolver.GetType().FullName}: The '{name}' struct is missing the '{member}'.");
    }

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetStructEventsData))]
    public void ValidateStructEvents(IResolver resolver, string name, string member)
    {
      var query = GetStruct(resolver, name);

      var hasEvent = query?.Events.Any(@event => @event.Name.Equals(member)) ?? false;

      Assert.True(hasEvent, $"{resolver.GetType().FullName}: The '{name}' struct is missing the '{member}'.");
    }

    [Theory]
    [Trait("Category", nameof(IDelegate))]
    [MemberData(nameof(GetStructDelegatesData))]
    public void ValidateStructDelegates(IResolver resolver, string name, string member)
    {
      var query = GetStruct(resolver, name);

      var hasDelegate = query?.Delegates.Any(@delegate => @delegate.Name.Equals(member)) ?? false;

      Assert.True(hasDelegate, $"{resolver.GetType().FullName}: The '{name}' struct is missing the '{member}'.");
    }

    [Theory]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetStructWithMembers))]
    public void ValidateStructPropertiesCount(IResolver resolver, string name)
    {
      var query = GetStruct(resolver, name);

      var propertiesCount = query?.Properties.Count ?? 0;

      Assert.True(propertiesCount == 1, $"{resolver.GetType().FullName}: The '{name}' struct has an unexpected number of members than expected.");
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetStructWithMembers))]
    public void ValidateStructMethodsCount(IResolver resolver, string name)
    {
      var query = GetStruct(resolver, name);

      var methodsCount = query?.Methods.Count ?? 0;

      Assert.True(methodsCount == 1, $"{resolver.GetType().FullName}: The '{name}' struct has an unexpected number of members than expected.");
    }

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetStructWithMembers))]
    public void ValidateStructEventsCount(IResolver resolver, string name)
    {
      var query = GetStruct(resolver, name);

      var eventsCount = query?.Events.Count ?? 0;

      Assert.True(eventsCount == 1, $"{resolver.GetType().FullName}: The '{name}' struct has an unexpected number of members than expected.");
    }

    [Theory]
    [Trait("Category", nameof(IDelegate))]
    [MemberData(nameof(GetStructWithMembers))]
    public void ValidateStructDelegatesCount(IResolver resolver, string name)
    {
      var query = GetStruct(resolver, name);

      var delegatesCount = query?.Delegates.Count ?? 0;

      Assert.True(delegatesCount == 1, $"{resolver.GetType().FullName}: The '{name}' struct has an unexpected number of members than expected.");
    }
  }
}
