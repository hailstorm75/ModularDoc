using MarkDoc.Members.Enums;

namespace MarkDoc.Members
{
  /// <summary>
  /// Interface for types
  /// </summary>
  public interface IType
  {
    /// <summary>
    /// Type name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Type namespace
    /// </summary>
    string TypeNamespace { get; }

    /// <summary>
    /// Type accessor
    /// </summary>
    AccessorType Accessor { get; }
  }
}
