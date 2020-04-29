using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkDoc.Elements.Markdown
{
  public class Page
    : BaseElement, IPage
  {
    private readonly IElementCreator m_creator;

    #region Properties

    public IReadOnlyCollection<IElement> Content { get; set; } = Enumerable.Empty<IElement>().ToArray();

    public IReadOnlyCollection<IPage> Subpages { get; set; } = Enumerable.Empty<IPage>().ToArray();

    /// <inheritdoc />
    public string Heading { get; set; } = string.Empty;

    /// <inheritdoc />
    public int Level { get; set; } = 0;

    #endregion

    public Page(IElementCreator creator)
    {
      m_creator = creator;
    }

    private void PrintTableOfContents(StringBuilder build)
    {
      IList CreateList(IEnumerable<IElement> elements, int indent)
      {
        var list = m_creator.CreateList();
        list.Content = elements.ToArray();
        list.Type = IList.ListType.Dotted;
        list.Level = indent;

        return list;
      }

      IEnumerable<IElement> GenerateTable(IPage page, int indent)
      {
        var text = m_creator.CreateText();
        text.Content = page.Heading;

        yield return text;

        foreach (var subpage in page.Subpages)
          yield return CreateList(GenerateTable(subpage, indent + 1), indent);
      }

      var tableOfContents = CreateList(GenerateTable(this, 0), 0);
      build.Append(tableOfContents.ToString());
    }

    /// <inheritdoc />
    public override string ToString()
    {
      var result = new StringBuilder();
      PrintTableOfContents(result);

      result.AppendJoin(string.Empty, Content.Select(x => x.ToString()));

      return result.ToString();
    }
  }
}
