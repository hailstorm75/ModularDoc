namespace MarkDoc.Members
{
  /// <summary>
  /// Interface for types
  /// </summary>
  public interface IType
    : IMember
  {
    /// <summary>
    /// Type namespace
    /// </summary>
    string TypeNamespace { get; }
  }
}
