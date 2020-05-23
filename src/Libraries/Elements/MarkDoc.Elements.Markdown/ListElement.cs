using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkDoc.Elements.Markdown
{
  public class ListElement
    : BaseElement, IList
  {
    #region Properties

    /// <inheritdoc />
    public IList.ListType Type { get; set; }

    /// <inheritdoc />
    public string Heading { get; set; } = string.Empty;

    /// <inheritdoc />
    public int Level { get; set; } = 0;

    /// <inheritdoc />
    public IReadOnlyCollection<IElement> Content { get; set; } = Enumerable.Empty<IElement>().ToArray();

    #endregion

    /// <inheritdoc />
    public override string ToString()
      => ToString(1);

    public string ToString(int indent)
    {
      var result = new StringBuilder();
      var index = 0;

      foreach (var item in Content)
      {
        if (item is IList list)
        {
          result.Append($"{list.ToString(indent + 1)}");
          continue;
        }

        result.Append($"{new string(' ', indent * 2 - 1)}");
        switch (Type)
        {
          case IList.ListType.Numbered:
            result.Append($"{++index}. ");
            break;
          case IList.ListType.Dotted:
            result.Append("- ");
            break;
        }

        result.Append($"{item}\n");
      }

      return result.ToString();
    }
  }
}
