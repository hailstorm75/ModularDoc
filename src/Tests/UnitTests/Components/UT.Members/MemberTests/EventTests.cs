using System;
using System.Collections.Generic;
using ModularDoc.Members.Enums;
using ModularDoc.Members.Members;
using ModularDoc.Members.Types;
using UT.Members.Data;
using Xunit;

namespace UT.Members.MemberTests
{
  public class EventTests
  {
    #region Data providers

    public static IEnumerable<object[]> GetEventNameData()
    {
      var filter = new HashSet<string> {Constants.EVENTS_CLASS, Constants.EVENTS_STRUCT, Constants.EVENTS_INTERFACE};
      var data = new [] {new object[] {Constants.EVENT_PUBLIC}};

      return data.ComposeData(
        resolver => resolver.FindMemberParents<IInterface>(Constants.EVENTS_NAMESPACE, filter),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetEventAccessorData()
    {
      var data = new[]
      {
        new object[] {Constants.EVENT_PUBLIC, AccessorType.Public},
        new object[] {Constants.EVENT_INTERNAL, AccessorType.Internal},
        new object[] {Constants.EVENT_PROTECTED, AccessorType.Protected},
        new object[] {Constants.EVENT_PROTECTED_INTERNAL, AccessorType.ProtectedInternal}
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IClass>(Constants.EVENTS_NAMESPACE, Constants.EVENTS_CLASS),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetEventInheritanceData()
    {
      var data = new []
      {
        new object[] { Constants.EVENT_NORMAL, MemberInheritance.Normal },
        new object[] { Constants.EVENT_OVERRIDE, MemberInheritance.Override },
        new object[] { Constants.EVENT_ABSTRACT, MemberInheritance.Abstract },
        new object[] { Constants.EVENT_VIRTUAL, MemberInheritance.Virtual },
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IClass>(Constants.EVENTS_NAMESPACE, Constants.EVENTS_CLASS_ABSTRACT),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetEventStaticData()
    {
      var data = new []
      {
        new object[] { Constants.EVENT_PUBLIC, false },
        new object[] { Constants.EVENT_STATIC, true },
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IClass>(Constants.EVENTS_NAMESPACE, Constants.EVENTS_CLASS),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetEventTypeData()
    {
      var data = new []
      {
        new object[] { Constants.EVENT_PUBLIC, nameof(EventHandler) },
      };

      return data.ComposeData(
        resolver => resolver.FindMemberParent<IClass>(Constants.EVENTS_NAMESPACE, Constants.EVENTS_CLASS),
        Constants.TEST_ASSEMBLY);
    }

    public static IEnumerable<object[]> GetEventRawNameData()
    {
      var data = new (string name, object[] data)[]
      {
        (Constants.PUBLIC_CLASS_EVENT_PARENT, new object[] { Constants.EVENT_PUBLIC_TOP, $"{Constants.EVENTS_NAMESPACE}.{Constants.PUBLIC_CLASS_EVENT_PARENT}.{Constants.EVENT_PUBLIC_TOP}" }),
        (Constants.PUBLIC_CLASS_EVENT_NESTED, new object[] { Constants.EVENT_PUBLIC_NESTED, $"{Constants.EVENTS_NAMESPACE}.{Constants.PUBLIC_CLASS_EVENT_PARENT}.{Constants.PUBLIC_CLASS_EVENT_NESTED}.{Constants.EVENT_PUBLIC_NESTED}" }),
        (Constants.PUBLIC_CLASS_EVENT_NESTED2, new object[] { Constants.EVENT_PUBLIC_NESTED2, $"{Constants.EVENTS_NAMESPACE}.{Constants.PUBLIC_CLASS_EVENT_PARENT}.{Constants.PUBLIC_CLASS_EVENT_NESTED}.{Constants.PUBLIC_CLASS_EVENT_NESTED2}.{Constants.EVENT_PUBLIC_NESTED2}" }),
      };

      return data.ComposeData(
        x => x.resolver.FindMemberParent<IClass>(Constants.EVENTS_NAMESPACE, x.typeName),
        Constants.TEST_ASSEMBLY);
    }

    private static IEvent? GetEvent(IInterface type, string name, bool throwIfNull = false)
      => type.Events.FindMember(name, throwIfNull);

    #endregion

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetEventNameData))]
    public void ValidateEventNames(IInterface type, string name)
    {
      var member = GetEvent(type, name);

      Assert.NotNull(member);
    }

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetEventRawNameData))]
    public void ValidateEventRawNames(IInterface type, string name, string rawName)
    {
      var member = GetEvent(type, name);

      Assert.Equal(rawName, member?.RawName);
    }

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetEventAccessorData))]
    public void ValidateEventAccessors(IInterface type, string name, AccessorType property)
    {
      var member = GetEvent(type, name);

      Assert.Equal(property, member?.Accessor);
    }

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetEventInheritanceData))]
    public void ValidateEventInheritance(IInterface type, string name, MemberInheritance inheritance)
    {
      var members = GetEvent(type, name, true);

      Assert.Equal(inheritance, members?.Inheritance);
    }

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetEventStaticData))]
    public void ValidateEventIsStatic(IInterface type, string name, bool isStatic)
    {
      var members = GetEvent(type, name, true);

      Assert.Equal(isStatic, members?.IsStatic);
    }

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetEventTypeData))]
    public void ValidateEventType(IInterface type, string name, string returns)
    {
      var members = GetEvent(type, name, true);

      Assert.Equal(returns, members?.Type.DisplayName);
    }
  }
}
