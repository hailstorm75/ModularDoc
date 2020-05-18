using System;
using System.Xml.Linq;
using static MarkDoc.Documentation.IDocMember;

namespace MarkDoc.Documentation.Xml
{
  public class DocMember
    : IDocMember
  {
    #region Properties

    public string Name { get; }

    public MemberType Type { get; }

    public IDocumentation Documentation { get; } 

    #endregion

    public DocMember(string name, char typeKey, XElement source)
    {
      if (source == null)
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
