using ModularDoc.Elements.Extensions;
using System.Collections.Generic;

namespace ModularDoc.Elements
{
  /// <summary>
  /// Interface for section elements
  /// </summary>
  public interface ISection
    : IElement, IHasContent<IReadOnlyCollection<IElement>>, IHasHeading
  {
  }
}
