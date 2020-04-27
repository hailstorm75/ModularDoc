using MarkDoc.Elements.Extensions;
using System.Collections.Generic;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for page elements
  /// </summary>
  public interface IPage
    : IElement, IHasContent<IReadOnlyCollection<IElement>>, IHasHeading
  {
  }
}
