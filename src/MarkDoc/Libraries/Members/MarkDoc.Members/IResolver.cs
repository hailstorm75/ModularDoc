namespace MarkDoc.Members
{
  /// <summary>
  /// Type resolver
  /// </summary>
  public interface IResolver
  {
    /// <summary>
    /// Resolves <paramref name="assembly"/> types
    /// </summary>
    /// <param name="assembly">Path to assembly</param>
    void Resolve(string assembly);
  }
}
