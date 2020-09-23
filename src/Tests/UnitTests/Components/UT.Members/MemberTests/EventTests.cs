using System;
using System.Collections.Generic;
using System.Linq;
using MarkDoc.Members;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Members;
using MarkDoc.Members.Types;
using UT.Members.Data;
using Xunit;

namespace UT.Members.MemberTests
{
  public class EventTests
  {
    #region Data providers

    public static IEnumerable<object[]> GetEventNameData()
    {
      var filter = new HashSet<string> { Constants.EVENTS_CLASS, Constants.EVENTS_STRUCT, Constants.EVENTS_INTERFACE };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var types = resolver?.Types.Value[Constants.EVENTS_NAMESPACE]
          .OfType<IInterface>()
          .Where(type => filter.Contains(type.Name));
        foreach (var type in types ?? throw new Exception())
          yield return new object[] { type, Constants.EVENT_PUBLIC };
      }
    }

    public static IEnumerable<object?[]> GetEventAccessorData()
    {
      var data = new object[]
      {
        new object[] {Constants.EVENT_PUBLIC, AccessorType.Public},
        new object[] {Constants.EVENT_INTERNAL, AccessorType.Internal},
        new object[] {Constants.EVENT_PROTECTED, AccessorType.Protected},
        new object[] {Constants.EVENT_PROTECTED_INTERNAL, AccessorType.ProtectedInternal}
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.EVENTS_NAMESPACE].OfType<IClass>().First(type => type.Name.Equals(Constants.EVENTS_CLASS));
        object?[] typeWrapper = { result };
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray();
      }
    }

    public static IEnumerable<object[]> GetEventInheritanceData()
    {
      var data = new object[]
      {
        new object[] { Constants.EVENT_NORMAL, MemberInheritance.Normal },
        new object[] { Constants.EVENT_OVERRIDE, MemberInheritance.Override },
        new object[] { Constants.EVENT_ABSTRACT, MemberInheritance.Abstract },
        new object[] { Constants.EVENT_VIRTUAL, MemberInheritance.Virtual },
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.EVENTS_NAMESPACE].OfType<IClass>().First(type => type.Name.Equals(Constants.EVENTS_CLASS_ABSTRACT));
        object?[] typeWrapper = { result };
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetEventStaticData()
    {
      var data = new object[]
      {
        new object[] { Constants.EVENT_PUBLIC, false },
        new object[] { Constants.EVENT_STATIC, true },
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.EVENTS_NAMESPACE].OfType<IClass>().First(type => type.Name.Equals(Constants.EVENTS_CLASS));
        object?[] typeWrapper = { result };
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetEventTypeData()
    {
      var data = new object[]
      {
        new object[] { Constants.EVENT_PUBLIC, nameof(EventHandler) },
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var result = resolver?.Types.Value[Constants.EVENTS_NAMESPACE].OfType<IClass>().First(type => type.Name.Equals(Constants.EVENTS_CLASS));
        object?[] typeWrapper = { result };
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> GetEventRawNameData()
    {
      var data = new (string name, object[] data)[]
      {
        (Constants.PUBLIC_CLASS_EVENT_PARENT, new object[] { Constants.EVENT_PUBLIC_TOP, $"{Constants.EVENTS_NAMESPACE}.{Constants.PUBLIC_CLASS_EVENT_PARENT}.{Constants.EVENT_PUBLIC_TOP}" }),
        (Constants.PUBLIC_CLASS_EVENT_NESTED, new object[] { Constants.EVENT_PUBLIC_NESTED, $"{Constants.EVENTS_NAMESPACE}.{Constants.PUBLIC_CLASS_EVENT_PARENT}.{Constants.PUBLIC_CLASS_EVENT_NESTED}.{Constants.EVENT_PUBLIC_NESTED}" }),
        (Constants.PUBLIC_CLASS_EVENT_NESTED2, new object[] { Constants.EVENT_PUBLIC_NESTED2, $"{Constants.EVENTS_NAMESPACE}.{Constants.PUBLIC_CLASS_EVENT_PARENT}.{Constants.PUBLIC_CLASS_EVENT_NESTED}.{Constants.PUBLIC_CLASS_EVENT_NESTED2}.{Constants.EVENT_PUBLIC_NESTED2}" }),
      };

      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        foreach (var (name, objects) in data)
        {
          var result = resolver?.Types.Value[Constants.EVENTS_NAMESPACE].OfType<IClass>().First(x => x.Name.Equals(name));
          object?[] typeWrapper = { result };
          yield return typeWrapper.Concat(objects).ToArray()!;
        }
      }
    }

    private static IEvent? GetEvent(IInterface type, string name, bool throwIfNull = false)
    {
      var member = type.Events.FirstOrDefault(ev => ev.Name.Equals(name, StringComparison.InvariantCulture));

      if (throwIfNull && member is null)
        throw new KeyNotFoundException();

      return member;
    }

    #endregion

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetEventNameData))]
    public void ValidatePropertyNames(IInterface type, string name)
    {
      var member = GetEvent(type, name);

      Assert.NotNull(member);
    }

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetEventRawNameData))]
    public void ValidatePropertyRawNames(IInterface type, string name, string rawName)
    {
      var member = GetEvent(type, name);

      Assert.Equal(rawName, member?.RawName);
    }

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetEventAccessorData))]
    public void ValidatePropertyAccessors(IClass type, string name, AccessorType property)
    {
      var member = GetEvent(type, name);

      Assert.Equal(property, member?.Accessor);
    }

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetEventInheritanceData))]
    public void ValidatePropertyInheritance(IClass type, string name, MemberInheritance inheritance)
    {
      var members = GetEvent(type, name, true);

      Assert.Equal(inheritance, members?.Inheritance);
    }

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetEventStaticData))]
    public void ValidatePropertyIsStatic(IClass type, string name, bool isStatic)
    {
      var members = GetEvent(type, name, true);

      Assert.Equal(isStatic, members?.IsStatic);
    }

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetEventTypeData))]
    public void ValidatePropertyType(IClass type, string name, string returns)
    {
      var members = GetEvent(type, name, true);

      Assert.Equal(returns, members?.Type.DisplayName);
    }
  }
}
