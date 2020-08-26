using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Autofac.Extras.Moq;
using MarkDoc.Members;
using MarkDoc.Members.ResolvedTypes;
using UT.Members.Data;
using Xunit;
using Constants = UT.Members.Data.Constants;

namespace UT.Members
{
  public class ResolverTests
  {
    private static IEnumerable<object?[]> GetInvalidAssemblyData()
    {
      var data = new[]
      {
        null,
        string.Empty,
        "../../InvalidAssembly.dll"
      };

      foreach (var path in data)
        foreach (var resolver in new ResolversProvider())
          yield return new[] { resolver.First(), path };
    }

    private static IEnumerable<object?[]> GetInvalidResolveTypeData()
    {
      var data = new[]
      {
        null,
        new object()
      };

      foreach (var item in data)
        foreach (var resolver in new ResolversProvider())
          yield return new[] { resolver.First(), item };
    }

    [Theory]
    [Category(nameof(IResolver))]
    [MemberData(nameof(GetInvalidAssemblyData))]
    public void ResolverInvalidAssembly(IResolver resolver, string? path)
      => Assert.ThrowsAny<Exception>(() => resolver.Resolve(path!));

    [Theory]
    [Category(nameof(IResolver))]
    [ClassData(typeof(ResolversProvider))]
    public void ResolverAccessTypesBeforeResolve(IResolver resolver)
    {
      var _ = resolver.Types.Value;

      Assert.ThrowsAny<Exception>(() => resolver.Resolve(Constants.TEST_ASSEMBLY));
    }

    [Theory]
    [Category(nameof(IResolver))]
    [MemberData(nameof(GetInvalidResolveTypeData))]
    public void ResolverResolveTypeInvalidSource(IResolver resolver, object source)
    {
      resolver.Resolve(Constants.TEST_ASSEMBLY);

      Assert.ThrowsAny<Exception>(() => resolver.Resolve(source));
    }

    [Theory]
    [Category(nameof(IResolver))]
    [MemberData(nameof(GetInvalidResolveTypeData))]
    public void ResolverFindReference(IResolver resolver, object source)
    {
      using var mock = AutoMock.GetLoose();
      var type = mock.Mock<IResType>().Object;

      resolver.Resolve(Constants.TEST_ASSEMBLY);

      Assert.ThrowsAny<Exception>(() => resolver.FindReference(source, type));
    }

    [Theory]
    [Category(nameof(IResolver))]
    [ClassData(typeof(ResolversProvider))]
    public void ResolverFindType(IResolver resolver)
    {
      resolver.Resolve(Constants.TEST_ASSEMBLY);

      Assert.ThrowsAny<Exception>(() => resolver.TryFindType(null!, out var _));
    }
  }
}
