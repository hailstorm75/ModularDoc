using System;
using System.Collections.Generic;
using System.Text;
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
    public override string ToString()
      => ToString(1);

    /// <inheritdoc />
    public string ToString(int indent)
    {
      var result = new StringBuilder();

      if (!string.IsNullOrEmpty(Heading))
        result.AppendLine(Heading.ToHeading(Level));

      var index = 0;

      foreach (var item in Content)
      {
        if (item is IList list)
        {
          result.Append(list.ToString(indent + 1));
          continue;
        }

        result.Append($"{new string(' ', indent * 2 - 1)}");
        switch (Type)
        {
          case ListType.Numbered:
            result.Append($"{++index}. ");
            break;
          case ListType.Dotted:
            result.Append("- ");
            break;
        }

        result.Append(item.ToString()).AppendLine();
      }

      return result.ToString();
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
          yield return list.ToString(indent + 1);
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
          yield return line;
      }
    }
  }
}
