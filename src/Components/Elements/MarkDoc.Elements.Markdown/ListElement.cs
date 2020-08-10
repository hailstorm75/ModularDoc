using System;
using System.Collections.Generic;
using MarkDoc.Helpers;
using static MarkDoc.Elements.IList;

namespace MarkDoc.Elements.Markdown
{
  public class ListElement
    : BaseElement, IList
  {
    #region Properties

    /// <inheritdoc />
    public ListType Type { get; }

    /// <inheritdoc />
    public string Heading { get; }

    /// <inheritdoc />
    public int Level { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IElement> Content { get; }

    #endregion

    public ListElement(IEnumerable<IElement> content, ListType type = ListType.Dotted, string heading = "", int level = 0)
    {
      Content = content.ToReadOnlyCollection();
      Type = type;
      Heading = heading;
      Level = level;
    }

    /// <inheritdoc />
    public override IEnumerable<string> Print()
      => Print(1);

    public IEnumerable<string> Print(int indent)
    {
      if (!string.IsNullOrEmpty(Heading))
      {
        yield return Heading.ToHeading(Level);
        yield return Environment.NewLine;
      }

      var index = 0;

      foreach (var item in Content)
      {
        if (item is IList list)
        {
          foreach (var line in list.Print(indent + 1))
            yield return line;

          continue;
        }

        yield return $"{new string(' ', indent * 2 - 1)}";
        switch (Type)
        {
          case ListType.Numbered:
            yield return $"{++index}. ";
            break;
          case ListType.Dotted:
            yield return "- ";
            break;
        }

        foreach (var line in item.Print())
        {
          yield return line;
          yield return Environment.NewLine;
        }
      }
    }
  }
}
