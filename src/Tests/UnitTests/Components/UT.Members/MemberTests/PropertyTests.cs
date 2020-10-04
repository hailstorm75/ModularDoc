using MarkDoc.Members.Members;
using MarkDoc.Members.Types;
using MarkDoc.Members.Enums;
using System.Collections.Generic;
using UT.Members.Data;
using Xunit;

namespace UT.Members.MemberTests
{
  public class PropertyTests
  {
    #region Data provider

    public static IEnumerable<object[]> GetPropertyGettersSettersData()
    {
      var filter = new HashSet<string> { Constants.PROPERTIES_CLASS, Constants.PROPERTIES_STRUCT, Constants.PROPERTIES_INTERFACE };
      var data = new[]
      {
        new object[] { Constants.PROPERTY_GET_SET, true, true },
        new object[] { Constants.PROPERTY_GET, true, false },
        new object[] { Constants.PROPERTY_SET, false, true }
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParents<IInterface>(Constants.PROPERTIES_NAMESPACE, filter),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetPropertyAccessorData()
    {
      var data = new[]
      {
        new object[] { Constants.PROPERTY_PUBLIC, AccessorType.Public, AccessorType.Public, AccessorType.Public },
        new object[] { Constants.PROPERTY_PROTECTED, AccessorType.Protected, AccessorType.Protected, AccessorType.Protected },
        new object[] { Constants.PROPERTY_INTERNAL, AccessorType.Internal, AccessorType.Internal, AccessorType.Internal },
        new object[] { Constants.PROPERTY_PRIVATE, null!, null!, null! },
        new object[] { Constants.PROPERTY_PUBLIC_GET_PROTECTED, AccessorType.Public, AccessorType.Protected, AccessorType.Public },
        new object[] { Constants.PROPERTY_PUBLIC_GET_INTERNAL, AccessorType.Public, AccessorType.Internal, AccessorType.Public },
        new object[] { Constants.PROPERTY_PUBLIC_GET_PRIVATE, AccessorType.Public, null!, AccessorType.Public },
        new object[] { Constants.PROPERTY_PUBLIC_SET_PROTECTED, AccessorType.Public, AccessorType.Public, AccessorType.Protected },
        new object[] { Constants.PROPERTY_PUBLIC_SET_INTERNAL, AccessorType.Public, AccessorType.Public, AccessorType.Internal },
        new object[] { Constants.PROPERTY_PUBLIC_SET_PRIVATE, AccessorType.Public, AccessorType.Public, null! },
        new object[] { Constants.PROPERTY_PROTECTED_INTERNAL, AccessorType.ProtectedInternal, AccessorType.ProtectedInternal, AccessorType.ProtectedInternal },
        new object[] { Constants.PROPERTY_PROTECTED_INTERNAL_GET_PRIVATE, AccessorType.ProtectedInternal, null!, AccessorType.ProtectedInternal },
        new object[] { Constants.PROPERTY_PROTECTED_INTERNAL_SET_PRIVATE, AccessorType.ProtectedInternal, AccessorType.ProtectedInternal, null! },
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IClass>(Constants.PROPERTIES_NAMESPACE, Constants.PROPERTIES_CLASS),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetPropertyNameData()
    {
      var filter = new HashSet<string> { Constants.PROPERTIES_CLASS, Constants.PROPERTIES_STRUCT, Constants.PROPERTIES_INTERFACE };
      var data = new[]
      {
        new object[] {Constants.PROPERTY_GET_SET},
        new object[] {Constants.PROPERTY_GET},
        new object[] {Constants.PROPERTY_SET}
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParents<IInterface>(Constants.PROPERTIES_NAMESPACE, filter),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetPropertyStaticData()
    {
      var data = new[]
      {
        new object[] { Constants.PROPERTY_PUBLIC, false },
        new object[] { Constants.PROPERTY_STATIC, true },
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IClass>(Constants.PROPERTIES_NAMESPACE, Constants.PROPERTIES_CLASS),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetPropertyValueData()
    {
      var filter = new HashSet<string> { Constants.PROPERTIES_CLASS, Constants.PROPERTIES_STRUCT, Constants.PROPERTIES_INTERFACE };
      var data = new[]
      {
        new object[] {Constants.PROPERTY_GET_SET, "string"},
        new object[] {Constants.PROPERTY_GET, "string"},
        new object[] {Constants.PROPERTY_SET, "string"}
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParents<IInterface>(Constants.PROPERTIES_NAMESPACE, filter),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetPropertiesInheritanceData()
    {
      var data = new[]
      {
        new object[] { Constants.PROPERTY_NORMAL, MemberInheritance.Normal },
        new object[] { Constants.PROPERTY_OVERRIDE, MemberInheritance.Override },
        new object[] { Constants.PROPERTY_ABSTRACT, MemberInheritance.Abstract },
        new object[] { Constants.PROPERTY_VIRTUAL, MemberInheritance.Virtual },
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IClass>(Constants.PROPERTIES_NAMESPACE, Constants.PROPERTIES_CLASS_ABSTRACT),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetPropertyRawNamesData()
    {
      var data = new (string name, object[] data)[]
      {
        (Constants.PUBLIC_CLASS_PROPERTY_PARENT, new object[] { Constants.PROPERTY_PUBLIC_TOP, $"{Constants.PROPERTIES_NAMESPACE}.{Constants.PUBLIC_CLASS_PROPERTY_PARENT}.{Constants.PROPERTY_PUBLIC_TOP}" }),
        (Constants.PUBLIC_CLASS_PROPERTY_NESTED, new object[] { Constants.PROPERTY_PUBLIC_NESTED, $"{Constants.PROPERTIES_NAMESPACE}.{Constants.PUBLIC_CLASS_PROPERTY_PARENT}.{Constants.PUBLIC_CLASS_PROPERTY_NESTED}.{Constants.PROPERTY_PUBLIC_NESTED}" }),
        (Constants.PUBLIC_CLASS_PROPERTY_NESTED2, new object[] { Constants.PROPERTY_PUBLIC_NESTED2, $"{Constants.PROPERTIES_NAMESPACE}.{Constants.PUBLIC_CLASS_PROPERTY_PARENT}.{Constants.PUBLIC_CLASS_PROPERTY_NESTED}.{Constants.PUBLIC_CLASS_PROPERTY_NESTED2}.{Constants.PROPERTY_PUBLIC_NESTED2}" }),
      };

      return data.ComposeData(
        x => x.resolver!.FindMemberParent<IClass>(Constants.PROPERTIES_NAMESPACE, x.typeName!),
        Constants.TEST_ASSEMBLY);
    }

    private static IProperty? GetProperty(IInterface type, string name, bool throwIfNull = false)
      => type.Properties.FindMember(name, throwIfNull);

    #endregion

    [Theory]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetPropertyGettersSettersData))]
    public void ValidatePropertyGettersSetters(IInterface type, string name, bool hasGet, bool hasSet)
    {
      var member = GetProperty(type, name, true);

      Assert.True((member?.GetAccessor != null) == hasGet && (member?.SetAccessor != null) == hasSet);
    }

    [Theory]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetPropertyAccessorData))]
    public void ValidatePropertyAccessors(IClass type, string name, AccessorType? property, AccessorType? getter, AccessorType? setter)
    {
      var member = GetProperty(type, name);

      Assert.Equal(property, member?.Accessor);
    }

    [Theory]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetPropertyAccessorData))]
    public void ValidatePropertyGetterAccessors(IClass type, string name, AccessorType? property, AccessorType? getter, AccessorType? setter)
    {
      var member = GetProperty(type, name);

      Assert.Equal(getter, member?.GetAccessor);
    }

    [Theory]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetPropertyNameData))]
    public void ValidatePropertyNames(IInterface type, string name)
    {
      var member = GetProperty(type, name);

      Assert.NotNull(member);
    }

    [Theory]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetPropertyRawNamesData))]
    public void ValidatePropertyRawNames(IInterface type, string name, string raw)
    {
      var member = GetProperty(type, name);

      Assert.Equal(raw, member?.RawName);
    }

    [Theory]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetPropertyStaticData))]
    public void ValidatePropertyIsStatic(IInterface type, string name, bool isStatic)
    {
      var member = GetProperty(type, name);

      Assert.Equal(isStatic, member?.IsStatic);
    }

    [Theory]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetPropertyAccessorData))]
    public void ValidatePropertySetterAccessors(IClass type, string name, AccessorType? property, AccessorType? getter, AccessorType? setter)
    {
      var member = GetProperty(type, name);

      Assert.Equal(setter, member?.SetAccessor);
    }

    [Theory]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetPropertyValueData))]
    public void ValidatePropertyValue(IInterface type, string name, string typeName)
    {
      var members = GetProperty(type, name, true);

      Assert.Equal(typeName, members?.Type.DisplayName);
    }

    [Theory]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetPropertiesInheritanceData))]
    public void ValidatePropertyInheritance(IClass type, string name, MemberInheritance inheritance)
    {
      var members = GetProperty(type, name, true);

      Assert.Equal(inheritance, members?.Inheritance);
    }
  }
}
