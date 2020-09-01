using System;
using System.Xml.Linq;
using static MarkDoc.Documentation.IDocMember;

namespace MarkDoc.Documentation.Xml
{
  /// <summary>
  /// Member documentation
  /// </summary>
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

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="name">Member name</param>
    /// <param name="typeKey">Member type key</param>
    /// <param name="source">Documentation source</param>
    internal DocMember(string name, char typeKey, XElement source)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      Name = name.Replace("/", ".", StringComparison.InvariantCultureIgnoreCase);
      Type = (MemberType)typeKey;
      Documentation = new DocumentationContent(source);
    }

    /// <summary>
    /// Explicit constructor
    /// </summary>
    /// <param name="name">Member name</param>
    /// <param name="type">Member type</param>
    /// <param name="documentation">Documentation source</param>
    internal DocMember(string name, MemberType type, IDocumentation documentation)
    {
      Name = name;
      Type = type;
      Documentation = documentation;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public bool Equals(DocMember other)
      => Name == other.Name && Type == other.Type && Documentation.Equals(other.Documentation);

    /// <inheritdoc />
    public override bool Equals(object? obj)
      => obj is DocMember other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
      => HashCode.Combine(Name, (int)Type, Documentation);

    public static bool operator ==(DocMember left, DocMember right)
      => left.Equals(right);

    public static bool operator !=(DocMember left, DocMember right)
      => !left.Equals(right);

    #endregion
  }
}
