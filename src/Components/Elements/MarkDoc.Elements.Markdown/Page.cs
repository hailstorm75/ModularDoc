using System;
using MarkDoc.Helpers;
using System.Collections.Generic;
using System.Linq;
using static MarkDoc.Elements.IList;

namespace MarkDoc.Elements.Markdown
{
  public class Page
    : BaseElement, IPage
  {
    private readonly IElementCreator m_creator;

    #region Properties

    /// <inheritdoc />
    public IReadOnlyCollection<IElement> Content { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IPage> Subpages { get; }

    /// <inheritdoc />
    public string Heading { get; }

    /// <inheritdoc />
    public int Level { get; }

    #endregion

    public Page(IElementCreator creator, IEnumerable<IElement> content, IEnumerable<IPage> subpages, string heading = "", int level = 0)
    {
      m_creator = creator;
      Content = content.ToReadOnlyCollection();
      Subpages = subpages.ToReadOnlyCollection();
      Heading = heading;
      Level = level;
    }

    /// <inheritdoc />
    public override IEnumerable<string> Print()
    {
      if (!string.IsNullOrEmpty(Heading))
      {
        yield return Heading.CleanInvalid().ToHeading(Level);
        yield return Environment.NewLine;
      }

      IList CreateList(IEnumerable<IElement> elements)
        => m_creator.CreateList(elements, ListType.Dotted);

      IEnumerable<IElement> GenerateTable(IPage page)
      {
        var text = m_creator.CreateText(page.Heading);

        yield return text;

        foreach (var subPage in page.Subpages)
          yield return CreateList(GenerateTable(subPage));
      }

      if (Subpages.Any())
      {
        var tableOfContents = CreateList(GenerateTable(this));
        foreach (var line in tableOfContents.Print())
          yield return line;
      }

      foreach (var item in Content)
      {
        yield return Environment.NewLine;
        foreach (var line in item.Print())
          yield return line;
      }
    }
  }
}
