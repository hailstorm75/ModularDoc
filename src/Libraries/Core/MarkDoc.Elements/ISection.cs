using MarkDoc.Elements.Extensions;
using System.Collections.Generic;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for section elements
  /// </summary>
  public interface ISection
    : IElement, IHasContent<IReadOnlyCollection<IElement>>, IHasHeading
  {
  }
}
