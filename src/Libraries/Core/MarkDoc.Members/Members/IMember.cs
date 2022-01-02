using MarkDoc.Members.Enums;

namespace MarkDoc.Members.Members
{
  /// <summary>
  /// Interface for type members
  /// </summary>
  public interface IMember
  {
    #region Properties

    /// <summary>
    /// Is method static
    /// </summary>
    bool IsStatic { get; }

    /// <summary>
    /// Member name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Raw member name
    /// </summary>
    string RawName { get; }

    /// <summary>
    /// Member accessor
    /// </summary>
    AccessorType Accessor { get; }

    /// <summary>
    /// Line number and source file of the given member
    /// </summary>
    (int line, string source)? LineSource { get; }

    #endregion
  }
}
