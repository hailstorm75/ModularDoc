using System;
using System.Xml.Linq;
using static MarkDoc.Documentation.IDocMember;

namespace MarkDoc.Documentation.Xml
{
  public class DocMember
    : IDocMember
  {
    #region Properties

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public MemberType Type { get; }

    /// <inheritdoc />
    public IDocumentation Documentation { get; } 

    #endregion

    public DocMember(string name, char typeKey, XElement source)
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));

      Name = name;
      Type = (MemberType)typeKey;
      Documentation = new DocumentationContent(source);
    }

    public DocMember(string name, MemberType type, IDocumentation documentation)
    {
      Name = name;
      Type = type;
      Documentation = documentation;
    }
  }
}
