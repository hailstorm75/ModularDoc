using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarkDoc.Documentation;
using MarkDoc.Documentation.Tags;
using MarkDoc.Helpers;
using MarkDoc.Members.Types;
using Moq;
using UT.Documentation.Data;
using Xunit;

namespace UT.Documentation.TagTests
{
  public class TextTagTests
  {
    #region Data

    public static IEnumerable<object[]> SimpleClassContentData()
    {
      var data = new object[]
      {
        new object[] {"Class summary", ITag.TagType.Summary},
        new object[] {"Class remarks", ITag.TagType.Remarks},
        new object[] {"Class example", ITag.TagType.Example},
        new object[] {"Class type param", ITag.TagType.Typeparam},
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        object?[] typeWrapper = {resolver};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> SimpleMethodContentData()
    {
      var data = new object[]
      {
        new object[] {"Method summary", ITag.TagType.Summary},
        new object[] {"Method remarks", ITag.TagType.Remarks},
        new object[] {"Method example", ITag.TagType.Example},
        new object[] {"Method return", ITag.TagType.Returns}
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        object?[] typeWrapper = {resolver};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> SimplePropertyContentData()
    {
      var data = new object[]
      {
        new object[] {"Property summary", ITag.TagType.Summary},
        new object[] {"Property value", ITag.TagType.Value}
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        object?[] typeWrapper = {resolver};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    #endregion

    [Theory]
    [Trait("Category", nameof(ITextTag))]
    [MemberData(nameof(SimplePropertyContentData))]
    public async Task TestSimplePropertyContent(IDocResolver resolver, string expectedContent, ITag.TagType tag)
    {
      // Assemble
      await resolver.ResolveAsync(Constants.TEST_DOCUMENTATION);

      var mocked = new Mock<IType>();
      mocked.SetupGet(x => x.RawName).Returns(Constants.SIMPLE_CLASS);
      resolver.TryFindType(mocked.Object, out var result);

      // Act
      var simpleMethod = result?.Members.Value[Constants.SIMPLE_PROPERTY];
      var documentation = simpleMethod?.Documentation.Tags[tag].First();
      var content = documentation?.Content.OfType<ITextTag>().First();

      // Assert
      Assert.Equal(expectedContent, content?.Content);
    }

    [Theory]
    [Trait("Category", nameof(ITextTag))]
    [MemberData(nameof(SimpleClassContentData))]
    public async Task TestSimpleClassContent(IDocResolver resolver, string expectedContent, ITag.TagType tag)
    {
      // Assemble
      await resolver.ResolveAsync(Constants.TEST_DOCUMENTATION);

      var mocked = new Mock<IType>();
      mocked.SetupGet(x => x.RawName).Returns(Constants.SIMPLE_CLASS);

      // Act
      resolver.TryFindType(mocked.Object, out var result);
      var documentation = result?.Documentation.Tags[tag].First();
      var content = documentation?.Content.OfType<ITextTag>().First();

      // Assert
      Assert.Equal(expectedContent, content?.Content);
    }

    [Theory]
    [Trait("Category", nameof(ITextTag))]
    [MemberData(nameof(SimpleMethodContentData))]
    public async Task TestSimpleMethodContent(IDocResolver resolver, string expectedContent, ITag.TagType tag)
    {
      // Assemble
      await resolver.ResolveAsync(Constants.TEST_DOCUMENTATION);

      var mocked = new Mock<IType>();
      mocked.SetupGet(x => x.RawName).Returns(Constants.SIMPLE_CLASS);
      resolver.TryFindType(mocked.Object, out var result);

      // Act
      var simpleMethod = result?.Members.Value[Constants.SIMPLE_METHOD];
      var documentation = simpleMethod?.Documentation.Tags[tag].First();
      var content = documentation?.Content.OfType<ITextTag>().First();

      // Assert
      Assert.Equal(expectedContent, content?.Content);
    }
  }
}