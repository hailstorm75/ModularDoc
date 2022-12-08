using System;
using ModularDoc.Helpers;
using System.Collections.Generic;
using System.Linq;
using static ModularDoc.Elements.IList;

namespace ModularDoc.Elements.Markdown
{
  /// <summary>
  /// Class for representing a markdown page
  /// </summary>
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

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="creator">Injected creator</param>
    /// <param name="content">Page content</param>
    /// <param name="subpages">Page sub-pages</param>
    /// <param name="heading">Element heading</param>
    /// <param name="level">Element heading level</param>
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
      // If there is a heading..
      if (!string.IsNullOrEmpty(Heading))
      {
        // print it
        yield return Heading.CleanInvalid().ToHeading(Level);
        // print a line break
        yield return Environment.NewLine;
      }

      IList CreateList(IEnumerable<IElement> elements)
        => m_creator.CreateList(elements, ListType.Dotted);

      IEnumerable<IElement> GenerateTable(IPage page)
      {
        // Print the heading text
        yield return m_creator.CreateText(page.Heading);

        // For every sub-page..
        foreach (var subPage in page.Subpages)
          // return a list of contents
          yield return CreateList(GenerateTable(subPage));
      }

      // If there are any sub-pages..
      if (Subpages.Any())
      {
        // create the table of contents
        var tableOfContents = CreateList(GenerateTable(this));
        // for each element of the table of contents..
        foreach (var line in tableOfContents.Print())
          // print it
          yield return line;
      }

      // For every element in the page content..
      foreach (var item in Content)
      {
        // print a line break
        yield return Environment.NewLine;
        // for every part of the element..
        foreach (var line in item.Print())
          // print it
          yield return line;
      }
    }
  }
}
