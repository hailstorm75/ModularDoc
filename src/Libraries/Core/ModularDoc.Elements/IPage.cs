using ModularDoc.Elements.Extensions;
using System.Collections.Generic;

namespace ModularDoc.Elements
{
  /// <summary>
  /// Interface for page elements
  /// </summary>
  public interface IPage
    : IElement, IHasContent<IReadOnlyCollection<IElement>>, IHasHeading
  {
    /// <summary>
    /// Pages within this given page
    /// </summary>
    /// <value>Collection of sub pages</value>
    public IReadOnlyCollection<IPage> Subpages { get; }
  }
}
