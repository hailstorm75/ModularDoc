using MarkDoc.Members.Members;
using MarkDoc.Members.Types;
using MarkDoc.Members;
using System.Collections.Generic;
using System.Linq;
using System;
using MarkDoc.Members.Enums;
using UT.Members.Data;
using Xunit;

namespace UT.Members.MemberTests
{
  public class PropertyTests
  {
    #region Data provider

    private static IEnumerable<object[]> GetPropertyGettersSettersData()
    {
      var filter = new HashSet<string> { Constants.PROPERTIES_CLASS, Constants.PROPERTIES_STRUCT, Constants.PROPERTIES_INTERFACE };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var types = resolver?.Types.Value[Constants.PROPERTIES_NAMESPACE]
          .OfType<IInterface>()
          .Where(type => filter.Contains(type.Name));
        foreach (var type in types ?? throw new Exception())
        {
          yield return new object[] { type, Constants.PROPERTY_GET_SET, true, true };
          yield return new object[] { type, Constants.PROPERTY_GET, true, false };
          yield return new object[] { type, Constants.PROPERTY_SET, false, true };
        }
      }
    }

    private static IEnumerable<object?[]> GetPropertyAccessorData()
    {
      var data = new object[]
      {
        new object[] { Constants.PROPERTY_PUBLIC, AccessorType.Public, AccessorType.Public, AccessorType.Public },
        new object[] { Constants.PROPERTY_PROTECTED, AccessorType.Protected, AccessorType.Protected, AccessorType.Protected },
        new object[] { Constants.PROPERTY_INTERNAL, AccessorType.Internal, AccessorType.Internal, AccessorType.Internal },
        new object?[] { Constants.PROPERTY_PRIVATE, null, null, null },
        new object[] { Constants.PROPERTY_PUBLIC_GET_PROTECTED, AccessorType.Public, AccessorType.Protected, AccessorType.Public },
        new object[] { Constants.PROPERTY_PUBLIC_GET_INTERNAL, AccessorType.Public, AccessorType.Internal, AccessorType.Public },
        new object?[] { Constants.PROPERTY_PUBLIC_GET_PRIVATE, AccessorType.Public, null, AccessorType.Public },
        new object[] { Constants.PROPERTY_PUBLIC_SET_PROTECTED, AccessorType.Public, AccessorType.Public, AccessorType.Protected },
        new object[] { Constants.PROPERTY_PUBLIC_SET_INTERNAL, AccessorType.Public, AccessorType.Public, AccessorType.Internal },
        new object?[] { Constants.PROPERTY_PUBLIC_SET_PRIVATE, AccessorType.Public, AccessorType.Public, null },
        new object[] { Constants.PROPERTY_PROTECTED_INTERNAL, AccessorType.ProtectedInternal, AccessorType.ProtectedInternal, AccessorType.ProtectedInternal },
        new object?[] { Constants.PROPERTY_PROTECTED_INTERNAL_GET_PRIVATE, AccessorType.ProtectedInternal, null, AccessorType.ProtectedInternal },
        new object?[] { Constants.PROPERTY_PROTECTED_INTERNAL_SET_PRIVATE, AccessorType.ProtectedInternal, AccessorType.ProtectedInternal, null },
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.PROPERTIES_NAMESPACE].OfType<IClass>().First(type => type.Name.Equals(Constants.PROPERTIES_CLASS));
        object?[] typeWrapper = { result };
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray();
      }
    }

    private static IEnumerable<object[]> GetPropertyNameData()
    {
      var filter = new HashSet<string> { Constants.PROPERTIES_CLASS, Constants.PROPERTIES_STRUCT, Constants.PROPERTIES_INTERFACE };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var types = resolver?.Types.Value[Constants.PROPERTIES_NAMESPACE]
          .OfType<IInterface>()
          .Where(type => filter.Contains(type.Name));
        foreach (var type in types ?? throw new Exception())
        {
          yield return new object[] { type, Constants.PROPERTY_GET_SET };
          yield return new object[] { type, Constants.PROPERTY_GET };
          yield return new object[] { type, Constants.PROPERTY_SET };
        }
      }
    }

    private static IEnumerable<object?[]> GetPropertyStaticData()
    {
      var data = new object[]
      {
        new object[] { Constants.PROPERTY_PUBLIC, false },
        new object[] { Constants.PROPERTY_STATIC, true },
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value["TestLibrary.Members.Properties"].OfType<IClass>().First(type => type.Name.Equals(Constants.PROPERTIES_CLASS));
        object?[] typeWrapper = { result };
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray();
      }
    }

    private static IEnumerable<object[]> GetPropertyValueData()
    {
      var filter = new HashSet<string> { Constants.PROPERTIES_CLASS, Constants.PROPERTIES_STRUCT, Constants.PROPERTIES_INTERFACE };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var types = resolver?.Types.Value[Constants.PROPERTIES_NAMESPACE]
          .OfType<IInterface>()
          .Where(type => filter.Contains(type.Name));
        foreach (var type in types ?? throw new Exception())
        {
          yield return new object[] { type, Constants.PROPERTY_GET_SET, "string" };
          yield return new object[] { type, Constants.PROPERTY_GET, "string" };
          yield return new object[] { type, Constants.PROPERTY_SET, "string" };
        }
      }
    }

    private static IEnumerable<object[]> GetPropertiesInheritanceData()
    {
      var data = new object[]
      {
        new object[] { Constants.PROPERTY_NORMAL, MemberInheritance.Normal },
        new object[] { Constants.PROPERTY_OVERRIDE, MemberInheritance.Override },
        new object[] { Constants.PROPERTY_ABSTRACT, MemberInheritance.Abstract },
        new object[] { Constants.PROPERTY_VIRTUAL, MemberInheritance.Virtual },
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.PROPERTIES_NAMESPACE].OfType<IClass>().First(type => type.Name.Equals(Constants.PROPERTIES_CLASS_ABSTRACT));
        object?[] typeWrapper = { result };
        foreach (object?[] entry in data)
          yield return typeWrapper?.Concat(entry).ToArray()!;
      }
    }

    private static IEnumerable<object[]> GetPropertyRawNamesData()
    {
      var data = new (string name, object[] data)[]
      {
        (Constants.PUBLIC_CLASS_PROPERTY_PARENT, new object[] { Constants.PROPERTY_PUBLIC_TOP, $"{Constants.PROPERTIES_NAMESPACE}.{Constants.PUBLIC_CLASS_PROPERTY_PARENT}.{Constants.PROPERTY_PUBLIC_TOP}" }),
        (Constants.PUBLIC_CLASS_PROPERTY_NESTED, new object[] { Constants.PROPERTY_PUBLIC_NESTED, $"{Constants.PROPERTIES_NAMESPACE}.{Constants.PUBLIC_CLASS_PROPERTY_PARENT}.{Constants.PUBLIC_CLASS_PROPERTY_NESTED}.{Constants.PROPERTY_PUBLIC_NESTED}" }),
        (Constants.PUBLIC_CLASS_PROPERTY_NESTED2, new object[] { Constants.PROPERTY_PUBLIC_NESTED2, $"{Constants.PROPERTIES_NAMESPACE}.{Constants.PUBLIC_CLASS_PROPERTY_PARENT}.{Constants.PUBLIC_CLASS_PROPERTY_NESTED}.{Constants.PUBLIC_CLASS_PROPERTY_NESTED2}.{Constants.PROPERTY_PUBLIC_NESTED2}" }),
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        foreach (var (name, objects) in data)
        {
          var result = resolver?.Types.Value[Constants.PROPERTIES_NAMESPACE].OfType<IClass>().First(x => x.Name.Equals(name));
          object?[] typeWrapper = { result };
          yield return typeWrapper.Concat(objects).ToArray()!;
        }
      }
    }

    private static IProperty? GetProperty(IInterface type, string name, bool throwIfNull = false)
    {
      var member = type.Properties.FirstOrDefault(prop => prop.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

      if (throwIfNull && member is null)
        throw new KeyNotFoundException();

      return member;
    }

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
