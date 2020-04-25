namespace MarkDoc.Members
{
  /// <summary>
  /// Interface for resolved types
  /// </summary>
  public interface IResType
  {
    /// <summary>
    /// Resolved type name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Resolved type namespace
    /// </summary>
    string TypeNamespace { get; }
  }
}
