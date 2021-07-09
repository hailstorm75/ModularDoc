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
  public class TagTests
  {
    #region Data

    public static IEnumerable<object[]> SimpleClassReferenceData()
    {
      var data = new object[]
      {
        new object[] {false, ITag.TagType.Summary},
        new object[] {false, ITag.TagType.Remarks},
        new object[] {false, ITag.TagType.Example},
        new object[] {true, ITag.TagType.Typeparam}
      };

      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        object?[] typeWrapper = {resolver};
        foreach (object?[] entry in data)
          yield return typeWrapper.Concat(entry).ToArray()!;
      }
    }

    public static IEnumerable<object[]> SimpleMethodReferenceData()
    {
      var data = new object[]
      {
        new object[] {false, ITag.TagType.Summary},
        new object[] {false, ITag.TagType.Remarks},
        new object[] {false, ITag.TagType.Example},
        new object[] {false, ITag.TagType.Returns},
        new object[] {true, ITag.TagType.Typeparam},
        new object[] {true, ITag.TagType.Param},
        new object[] {true, ITag.TagType.Exception},
        new object[] {true, ITag.TagType.Seealso}
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
    [MemberData(nameof(SimpleClassReferenceData))]
    public async Task TestSimpleClassReference(IDocResolver resolver, bool hasReference, ITag.TagType tag)
    {
      // Assemble
      await resolver.ResolveAsync(Constants.TEST_DOCUMENTATION);

      var mocked = new Mock<IType>();
      mocked.SetupGet(x => x.RawName).Returns(Constants.SIMPLE_CLASS);

      // Act
      resolver.TryFindType(mocked.Object, out var result);
      var documentation = result?.Documentation.Tags[tag].First();

      // Assert
      Assert.Equal(hasReference, !string.IsNullOrEmpty(documentation?.Reference));
    }

    [Theory]
    [Trait("Category", nameof(ITextTag))]
    [MemberData(nameof(SimpleMethodReferenceData))]
    public async Task TestSimpleMethodContent(IDocResolver resolver, bool hasReference, ITag.TagType tag)
    {
      // Assemble
      await resolver.ResolveAsync(Constants.TEST_DOCUMENTATION);

      var mocked = new Mock<IType>();
      mocked.SetupGet(x => x.RawName).Returns(Constants.SIMPLE_CLASS);
      resolver.TryFindType(mocked.Object, out var result);

      // Act
      var simpleMethod = result?.Members.Value[Constants.SIMPLE_METHOD];
      var documentation = simpleMethod?.Documentation.Tags[tag].First();

      // Assert
      Assert.Equal(hasReference, !string.IsNullOrEmpty(documentation?.Reference));
    }
  }
}