namespace MarkDoc.Members
{
  /// <summary>
  /// Type resolver
  /// </summary>
  public interface IResolver
  {
    /// <summary>
    /// Resolves a <paramref name="subject"/> to a type
    /// </summary>
    /// <param name="subject">Subject to resolve</param>
    /// <returns>Resolved subject</returns>
    IType Resolve(object subject);
  }
}
