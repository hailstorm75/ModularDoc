using System;
using System.Xml.Linq;

namespace MarkDoc.Documentation.Xml
{
  public class DocType
    : IDocType
  {
    #region Properties

    /// <inheritdoc />
    public IDocumentation Documentation { get; }

    #endregion

    public DocType(XElement source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      Documentation = new DocumentationContent(source);
    }
  }
}
