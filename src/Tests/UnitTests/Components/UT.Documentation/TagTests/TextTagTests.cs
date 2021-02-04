using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarkDoc.Documentation;
using MarkDoc.Documentation.Tags;
using MarkDoc.Members.Types;
using Moq;
using UT.Documentation.Data;
using Xunit;

namespace UT.Documentation.TagTests
{
  public class TextTagTests
  {
    public static IEnumerable<object[]> SimpleSummaryData()
      => new ResolversProvider().Select(resolver => new object[] {resolver});

    [Theory]
    [Trait("Category", nameof(ITextTag))]
    [MemberData(nameof(SimpleSummaryData))]
    public async Task TestSimpleSummary(IDocResolver resolver)
    {
      // Assemble
      await resolver.ResolveAsync(Constants.TEST_DOCUMENTATION);

      var mocked = new Mock<IType>();
      mocked.SetupGet(x => x.RawName).Returns("TestDocLibrary.SimpleClass");

      // Act
      resolver.TryFindType(mocked.Object, out var result);
      var summary = result?.Documentation.Tags[ITag.TagType.Summary].First();
      var content = summary?.Content.OfType<ITextTag>().First();

      // Assert
      Assert.Equal("Class summary", content?.Content);
    }
  }
}