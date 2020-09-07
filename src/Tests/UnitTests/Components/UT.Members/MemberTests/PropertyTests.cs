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
      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var types = resolver?.Types.Value["TestLibrary.Members.Properties"].OfType<IInterface>();
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
        new object?[] { Constants.PROPERTY_PUBLIC_SET_PRIVATE, AccessorType.Public, AccessorType.Public, null }
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value["TestLibrary.Members.Properties"].OfType<IClass>().First(type => type.Name.Equals("ClassProperties"));
        object?[] typeWrapper = { result };
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray();
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
    [MemberData(nameof(GetPropertyAccessorData))]
    public void ValidatePropertySetterAccessors(IClass type, string name, AccessorType? property, AccessorType? getter, AccessorType? setter)
    {
      var member = GetProperty(type, name);

      Assert.Equal(setter, member?.SetAccessor);
    }
  }
}
