using MarkDoc.Elements.Extensions;
using System.Collections.Generic;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for the table element
  /// </summary>
  public interface ITable
    : IElement, IHasContent<IReadOnlyCollection<IReadOnlyCollection<IElement>>>, IHasHeading
  {
    IReadOnlyCollection<IText> Headings { get; set; }
  }
}
