using System;
using System.Xml.Linq;
using static MarkDoc.Documentation.IDocMember;

namespace MarkDoc.Documentation.Xml
{
  public readonly struct DocMember
    : IDocMember, IEquatable<DocMember>
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

    /// <inheritdoc />
    public bool Equals(DocMember other)
      => Name == other.Name && Type == other.Type && Documentation.Equals(other.Documentation);

    /// <inheritdoc />
    public override bool Equals(object? obj)
      => obj is DocMember other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
      => HashCode.Combine(Name, (int) Type, Documentation);

    public static bool operator ==(DocMember left, DocMember right)
      => left.Equals(right);

    public static bool operator !=(DocMember left, DocMember right)
      => !left.Equals(right);
  }
}
