using System;
using System.Collections.Generic;
using System.Linq;
using MarkDoc.Helpers;
using MarkDoc.Members;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Members;
using MarkDoc.Members.Types;
using UT.Members.Data;
using Xunit;

namespace UT.Members.TypeTests
{
  public class InterfaceTests
  {
    #region Data providers

    public static IEnumerable<object?[]> GetInterfaceNamesData()
    {
      var data = new[]
      {
        Constants.PUBLIC_INTERFACE,
        Constants.INTERNAL_INTERFACE,
        Constants.PUBLIC_NESTED_INTERFACE,
        Constants.PROTECTED_NESTED_INTERFACE,
        Constants.PROTECTED_INTERNAL_NESTED_INTERFACE,
        Constants.INTERNAL_NESTED_INTERFACE,
        Constants.PUBLIC_GENERIC_INTERFACE,
        Constants.PUBLIC_NESTED_GENERIC_INTERFACE
      };

      return data.ComposeData();
    }

    public static IEnumerable<object[]> GetInterfaceAccessorsData()
    {
      var data = new[]
      {
        new object[] {Constants.PUBLIC_INTERFACE, AccessorType.Public},
        new object[] {Constants.INTERNAL_INTERFACE, AccessorType.Internal},
        new object[] {Constants.PUBLIC_NESTED_INTERFACE, AccessorType.Public},
        new object[] {Constants.PROTECTED_NESTED_INTERFACE, AccessorType.Protected},
        new object[] {Constants.PROTECTED_INTERNAL_NESTED_INTERFACE, AccessorType.ProtectedInternal},
        new object[] {Constants.INTERNAL_NESTED_INTERFACE, AccessorType.Internal},
      };

      return data.ComposeData();
    }

    public static IEnumerable<object[]> GetInterfaceNamespaceData()
    {
      const string interfaceNameSpace = "TestLibrary.Interfaces";
      var data = new[]
      {
        new object[] {Constants.PUBLIC_INTERFACE, interfaceNameSpace},
        new object[] {Constants.INTERNAL_INTERFACE, interfaceNameSpace},
        new object[] {Constants.PUBLIC_NESTED_INTERFACE, $"{interfaceNameSpace}.InterfaceParent"},
        new object[] {Constants.PROTECTED_NESTED_INTERFACE, $"{interfaceNameSpace}.InterfaceParent"},
        new object[] {Constants.PROTECTED_INTERNAL_NESTED_INTERFACE, $"{interfaceNameSpace}.InterfaceParent"},
        new object[] {Constants.INTERNAL_NESTED_INTERFACE, $"{interfaceNameSpace}.InterfaceParent"},
      };

      return data.ComposeData();
    }

    public static IEnumerable<object[]> GetInterfaceGenericData()
    {
      var generics = new Dictionary<string, (Variance, IReadOnlyCollection<string>)>
      {
        { "T1", (Variance.NonVariant, new string[]{}) },
        { "T2", (Variance.Contravariant, new []{ Constants.PUBLIC_INTERFACE }) },
        { "T3", (Variance.Covariant, new []{ nameof(IDisposable) }) },
      };

      var data = new[]
      {
        new object[] { Constants.PUBLIC_GENERIC_INTERFACE, generics },
        new object[] { Constants.PUBLIC_NESTED_GENERIC_INTERFACE, generics }
      };

      return data.ComposeData();
    }

    public static IEnumerable<object[]> GetInterfacePropertiesData()
      => GetInterfaceWithMembersData("Property");

    public static IEnumerable<object[]> GetInterfaceMethodsData()
      => GetInterfaceWithMembersData("Method");

    public static IEnumerable<object[]> GetInterfaceEventsData()
      => GetInterfaceWithMembersData("Event");

    public static IEnumerable<object[]> GetInterfaceDelegatesData()
      => GetInterfaceWithMembersData("Delegate");

    public static IEnumerable<object[]> GetInterfaceWithMembersData(string name)
      => new ResolversProvider().Select(resolver => new[] { resolver.First(), Constants.PUBLIC_INTERFACE, name });

    public static IEnumerable<object[]> GetInterfaceWithMembersData()
      => new ResolversProvider().Select(resolver => new[] { resolver.First(), Constants.PUBLIC_INTERFACE });

    private static IEnumerable<object[]> GetInterfaceInheritedMembersData(string member)
    {
      var data = new[]
      {
        new object[] {Constants.PUBLIC_INHERITING_INTERFACE, member, Constants.PUBLIC_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_INTERFACE_EMPTY, member, Constants.PUBLIC_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_INTERFACE, member, Constants.PUBLIC_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_INTERFACE_EMPTY, member, Constants.PUBLIC_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_INTERFACE, $"Other{member}", Constants.PUBLIC_INHERITING_AND_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_INTERFACE_EMPTY, $"Other{member}", Constants.PUBLIC_INHERITING_AND_INHERITED_INTERFACE},
      };

      return data.ComposeData();
    }

    public static IEnumerable<object[]> GetInterfaceInheritedEventsData()
      => GetInterfaceInheritedMembersData("Event");

    public static IEnumerable<object[]> GetInterfaceInheritedDelegatesData()
      => GetInterfaceInheritedMembersData("Delegate");

    public static IEnumerable<object[]> GetInterfaceInheritedPropertiesData()
      => GetInterfaceInheritedMembersData("Property");

    public static IEnumerable<object[]> GetInterfaceInheritedMethodsData()
      => GetInterfaceInheritedMembersData("Method");

    private static IInterface? GetInterface(IResolver resolver, string name)
    {
      resolver.Resolve(Constants.TEST_ASSEMBLY);

      return resolver
        .GetTypes<IInterface>()
        .FirstOrDefault(type => type.Name.Equals(name));
    }

    #endregion

    [Theory]
    [Trait("Category", nameof(IInterface))]
    [MemberData(nameof(GetInterfaceNamesData))]
    public void ValidateInterfaceNames(IResolver resolver, string name)
    {
      var query = GetInterface(resolver, name);

      Assert.NotNull(query);
    }

    [Theory]
    [Trait("Category", nameof(IInterface))]
    [MemberData(nameof(GetInterfaceNamespaceData))]
    public void ValidateInterfaceNamespaces(IResolver resolver, string name, string expectedNamespace)
    {
      var query = GetInterface(resolver, name);

      Assert.Equal(expectedNamespace, query?.TypeNamespace);
    }

    [Theory]
    [Trait("Category", nameof(IInterface))]
    [MemberData(nameof(GetInterfaceAccessorsData))]
    public void ValidateInterfaceAccessors(IResolver resolver, string name, AccessorType accessor)
    {
      var query = GetInterface(resolver, name);

      Assert.True(query?.Accessor == accessor, $"{resolver.GetType().FullName}: The '{name}' accessor type is invalid. Expected '{accessor}' != Actual '{query?.Accessor}'");
    }

    [Theory]
    [Trait("Category", nameof(IInterface))]
    [MemberData(nameof(GetInterfaceNamespaceData))]
    public void ValidateInterfaceRawName(IResolver resolver, string name, string expectedNamespace)
    {
      var query = GetInterface(resolver, name);

      Assert.True(query?.RawName.Equals($"{expectedNamespace}.{name}"), $"{resolver.GetType().FullName}: The '{name}' raw name is invalid. Expected '{expectedNamespace}.{name}' != Actual '{query?.RawName}'.");
    }

    [Theory]
    [Trait("Category", nameof(IInterface))]
    [MemberData(nameof(GetInterfaceGenericData))]
    public void ValidateInterfaceGenericVariances(IResolver resolver, string name, Dictionary<string, (Variance variance, IReadOnlyCollection<string>)> generics)
    {
      var query = GetInterface(resolver, name);

      var interfaceGenerics = query?.Generics
        .Select(item => (item.Key, item.Value.variance))
        .OrderBy(key => key.Key);

      var expectedGenerics = generics
        .Select(item => (item.Key, item.Value.variance))
        .OrderBy(key => key.Key);

      Assert.Equal(expectedGenerics, interfaceGenerics);
    }

    [Theory]
    [Trait("Category", nameof(IInterface))]
    [MemberData(nameof(GetInterfaceGenericData))]
    public void ValidateInterfaceGenericConstraints(IResolver resolver, string name, Dictionary<string, (Variance, IReadOnlyCollection<string> constraints)> generics)
    {
      var query = GetInterface(resolver, name);

      var actualGenerics = query?.Generics
        .Select(item => (item.Key, string.Join(";", item.Value.constraints.Select(constraint => constraint.DisplayName).OrderBy(Linq.XtoX))))
        .OrderBy(key => key.Key);

      var expectedGenerics = generics
        .Select(item => (item.Key, string.Join(";", item.Value.constraints.OrderBy(Linq.XtoX))))
        .OrderBy(key => key.Key);

      Assert.Equal(expectedGenerics, actualGenerics);
    }

    [Theory]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetInterfacePropertiesData))]
    public void ValidateInterfaceProperties(IResolver resolver, string name, string member)
    {
      var query = GetInterface(resolver, name);

      var hasProperty = query?.Properties.Any(property => property.Name.Equals(member)) ?? false;

      Assert.True(hasProperty, $"{resolver.GetType().FullName}: The '{name}' interface is missing the '{member}'.");
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetInterfaceMethodsData))]
    public void ValidateInterfaceMethods(IResolver resolver, string name, string member)
    {
      var query = GetInterface(resolver, name);

      var hasMethod = query?.Methods.Any(method => method.Name.Equals(member)) ?? false;

      Assert.True(hasMethod, $"{resolver.GetType().FullName}: The '{name}' interface is missing the '{member}'.");
    }

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetInterfaceEventsData))]
    public void ValidateInterfaceEvents(IResolver resolver, string name, string member)
    {
      var query = GetInterface(resolver, name);

      var hasEvent = query?.Events.Any(@event => @event.Name.Equals(member)) ?? false;

      Assert.True(hasEvent, $"{resolver.GetType().FullName}: The '{name}' interface is missing the '{member}'.");
    }

    [Theory]
    [Trait("Category", nameof(IDelegate))]
    [MemberData(nameof(GetInterfaceDelegatesData))]
    public void ValidateInterfaceDelegates(IResolver resolver, string name, string member)
    {
      var query = GetInterface(resolver, name);

      var hasDelegate = query?.Delegates.Any(@delegate => @delegate.Name.Equals(member)) ?? false;

      Assert.True(hasDelegate, $"{resolver.GetType().FullName}: The '{name}' interface is missing the '{member}'.");
    }

    [Theory]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetInterfaceWithMembersData))]
    public void ValidateInterfacePropertiesCount(IResolver resolver, string name)
    {
      var query = GetInterface(resolver, name);

      var propertiesCount = query?.Properties.Count ?? 0;

      Assert.True(propertiesCount == 1, $"{resolver.GetType().FullName}: The '{name}' interface has an unexpected number of members than expected.");
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetInterfaceWithMembersData))]
    public void ValidateInterfaceMethodsCount(IResolver resolver, string name)
    {
      var query = GetInterface(resolver, name);

      var methodsCount = query?.Methods.Count ?? 0;

      Assert.True(methodsCount == 1, $"{resolver.GetType().FullName}: The '{name}' interface has an unexpected number of members than expected.");
    }

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetInterfaceWithMembersData))]
    public void ValidateInterfaceEventsCount(IResolver resolver, string name)
    {
      var query = GetInterface(resolver, name);

      var eventsCount = query?.Events.Count ?? 0;

      Assert.True(eventsCount == 1, $"{resolver.GetType().FullName}: The '{name}' interface has an unexpected number of members than expected.");
    }

    [Theory]
    [Trait("Category", nameof(IDelegate))]
    [MemberData(nameof(GetInterfaceWithMembersData))]
    public void ValidateInterfaceDelegatesCount(IResolver resolver, string name)
    {
      var query = GetInterface(resolver, name);

      var delegatesCount = query?.Delegates.Count ?? 0;

      Assert.True(delegatesCount == 1, $"{resolver.GetType().FullName}: The '{name}' interface has an unexpected number of members than expected.");
    }

    [Theory]
    [Trait("Category", nameof(IInterface))]
    [MemberData(nameof(GetInterfaceWithMembersData))]
    public void ValidateInterfaceInheritedInterfaces(IResolver resolver, string name)
    {
      var query = GetInterface(resolver, name);

      var type = query?.InheritedInterfaces.FirstOrDefault(inherited => inherited.DisplayName.Equals(nameof(IDisposable)));

      Assert.False(type is null, $"{resolver.GetType().FullName}: The '{name}' interface is missing the expected inherited interface.");
    }

    [Theory]
    [Trait("Category", nameof(IInterface))]
    [MemberData(nameof(GetInterfaceWithMembersData))]
    public void ValidateInterfaceInheritedInterfacesCount(IResolver resolver, string name)
    {
      var query = GetInterface(resolver, name);

      var interfacesCount = query?.InheritedInterfaces.Count ?? 0;

      Assert.True(interfacesCount == 1, $"{resolver.GetType().FullName}: The '{name}' interface has an unexpected number of inherited interface.");
    }

    [Theory]
    [Trait("Category", nameof(IEnum))]
    [MemberData(nameof(GetInterfaceWithMembersData))]
    public void ValidateInterfaceNestedTypeEnum(IResolver resolver, string name)
    {
      var query = GetInterface(resolver, name);

      var enumType = query?.NestedTypes.OfType<IEnum>().FirstOrDefault(nested => nested.Name.Equals("MyEnum"));

      Assert.False(enumType is null, $"{resolver.GetType().FullName}: The '{name}' interface is missing the expected nested enum.");
    }

    [Theory]
    [Trait("Category", nameof(IEnum))]
    [MemberData(nameof(GetInterfaceWithMembersData))]
    public void ValidateInterfaceNestedTypeEnumCount(IResolver resolver, string name)
    {
      var query = GetInterface(resolver, name);

      var enumCount = query?.NestedTypes.OfType<IEnum>().Count() ?? 0;

      Assert.True(enumCount == 1, $"{resolver.GetType().FullName}: The '{name}' interface has an unexpected number of nested enums.");
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [MemberData(nameof(GetInterfaceWithMembersData))]
    public void ValidateInterfaceNestedTypeStruct(IResolver resolver, string name)
    {
      var query = GetInterface(resolver, name);

      var structType = query?.NestedTypes.OfType<IStruct>().FirstOrDefault(nested => nested.Name.Equals("MyStruct"));

      Assert.False(structType is null, $"{resolver.GetType().FullName}: The '{name}' interface is missing the expected nested struct.");
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [MemberData(nameof(GetInterfaceWithMembersData))]
    public void ValidateInterfaceNestedTypeStructCount(IResolver resolver, string name)
    {
      var query = GetInterface(resolver, name);

      var structCount = query?.NestedTypes.OfType<IStruct>().Count() ?? 0;

      Assert.True(structCount == 1, $"{resolver.GetType().FullName}: The '{name}' interface has an unexpected number of nested structs.");
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetInterfaceWithMembersData))]
    public void ValidateInterfaceNestedTypeClass(IResolver resolver, string name)
    {
      var query = GetInterface(resolver, name);

      var classType = query?.NestedTypes.OfType<IClass>().FirstOrDefault(nested => nested.Name.Equals("MyClass"));

      Assert.False(classType is null, $"{resolver.GetType().FullName}: The '{name}' interface is missing the expected nested class.");
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetInterfaceWithMembersData))]
    public void ValidateInterfaceNestedTypeClassCount(IResolver resolver, string name)
    {
      var query = GetInterface(resolver, name);

      var classCount = query?.NestedTypes.OfType<IClass>().Count() ?? 0;

      Assert.True(classCount == 1, $"{resolver.GetType().FullName}: The '{name}' interface has an unexpected number of nested classes.");
    }

    [Theory]
    [Trait("Category", nameof(IInterface))]
    [MemberData(nameof(GetInterfaceWithMembersData))]
    public void ValidateInterfaceNestedTypeInterface(IResolver resolver, string name)
    {
      var query = GetInterface(resolver, name);

      var interfaceType = query?.NestedTypes.OfType<IInterface>().FirstOrDefault(nested => nested.Name.Equals("IMyInterface"));

      Assert.False(interfaceType is null, $"{resolver.GetType().FullName}: The '{name}' interface is missing the expected nested interface.");
    }

    [Theory]
    [Trait("Category", nameof(IInterface))]
    [MemberData(nameof(GetInterfaceWithMembersData))]
    public void ValidateInterfaceNestedTypeInterfaceCount(IResolver resolver, string name)
    {
      var query = GetInterface(resolver, name);

      var interfaceCount= query?.NestedTypes.OfType<IInterface>().Count(item => !(item is IClass || item is IStruct)) ?? 0;

      Assert.True(interfaceCount == 1, $"{resolver.GetType().FullName}: The '{name}' interface has an unexpected number of nested interfaces.");
    }

    [Theory]
    [Trait("Category", nameof(IInterface))]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetInterfaceInheritedEventsData))]
    public void ValidateInheritedEventMembers(IResolver resolver, string name, string memberName, string sourceTypeName)
    {
      var query = GetInterface(resolver, name);

      var inheritedMember = query?.InheritedTypes.Value.FirstOrDefault(member => member.Key.Name.Equals(memberName)) ?? default;

      Assert.Equal(sourceTypeName, inheritedMember.Value.Name);
    }

    [Theory]
    [Trait("Category", nameof(IInterface))]
    [Trait("Category", nameof(IDelegate))]
    [MemberData(nameof(GetInterfaceInheritedDelegatesData))]
    public void ValidateInheritedDelegateMembers(IResolver resolver, string name, string memberName, string sourceTypeName)
    {
      var query = GetInterface(resolver, name);

      var inheritedMember = query?.InheritedTypes.Value.FirstOrDefault(member => member.Key.Name.Equals(memberName)) ?? default;

      Assert.Equal(sourceTypeName, inheritedMember.Value.Name);
    }

    [Theory]
    [Trait("Category", nameof(IInterface))]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetInterfaceInheritedPropertiesData))]
    public void ValidateInheritedPropertyMembers(IResolver resolver, string name, string memberName, string sourceTypeName)
    {
      var query = GetInterface(resolver, name);

      var inheritedMember = query?.InheritedTypes.Value.FirstOrDefault(member => member.Key.Name.Equals(memberName)) ?? default;

      Assert.Equal(sourceTypeName, inheritedMember.Value.Name);
    }

    [Theory]
    [Trait("Category", nameof(IInterface))]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetInterfaceInheritedMethodsData))]
    public void ValidateInheritedMethodMembers(IResolver resolver, string name, string memberName, string sourceTypeName)
    {
      var query = GetInterface(resolver, name);

      var inheritedMember = query?.InheritedTypes.Value.FirstOrDefault(member => member.Key.Name.Equals(memberName)) ?? default;

      Assert.Equal(sourceTypeName, inheritedMember.Value.Name);
    }
  }
}