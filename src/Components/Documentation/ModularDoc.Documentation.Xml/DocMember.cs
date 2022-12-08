using System;
using System.Xml.Linq;
using static ModularDoc.Documentation.IDocMember;

namespace ModularDoc.Documentation.Xml
{
  /// <summary>
  /// Member documentation
  /// </summary>
  public readonly struct DocMember
    : IDocMember, IEquatable<DocMember>
  {
    #region Properties

    /// <inheritdoc />
    public string RawName { get; }

    /// <inheritdoc />
    public string DisplayName { get; }

    /// <inheritdoc />
    public MemberType Type { get; }

    /// <inheritdoc />
    public IDocumentation Documentation { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="rawName">Member raw name</param>
    /// <param name="displayName">Member display name</param>
    /// <param name="typeKey">Member type key</param>
    /// <param name="source">Documentation source</param>
    internal DocMember(string rawName, string displayName, char typeKey, XElement source)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      RawName = rawName.Replace("/", ".", StringComparison.InvariantCultureIgnoreCase);
      DisplayName = displayName;
      Type = (MemberType)typeKey;
      Documentation = new DocumentationContent(source);
    }

    /// <summary>
    /// Explicit constructor
    /// </summary>
    /// <param name="rawName">Member name</param>
    /// <param name="displayName">Member display name</param>
    /// <param name="type">Member type</param>
    /// <param name="documentation">Documentation source</param>
    internal DocMember(string rawName, string displayName, MemberType type, IDocumentation documentation)
    {
      RawName = rawName;
      DisplayName = displayName;
      Type = type;
      Documentation = documentation;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public bool Equals(DocMember other)
      => RawName == other.RawName && Type == other.Type && Documentation.Equals(other.Documentation);

    /// <inheritdoc />
    public override bool Equals(object? obj)
      => obj is DocMember other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
      => HashCode.Combine(RawName, (int)Type, Documentation);

    public static bool operator ==(DocMember left, DocMember right)
      => left.Equals(right);

    public static bool operator !=(DocMember left, DocMember right)
      => !left.Equals(right);

    #endregion
  }
}
