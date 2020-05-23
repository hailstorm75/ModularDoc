namespace MarkDoc.Documentation
{
  /// <summary>
  /// Interface for member documentation
  /// </summary>
  public interface IDocMember
  {
    /// <summary>
    /// Member types
    /// </summary>
    public enum MemberType
    {
      /// <summary>
      /// Member is a method
      /// </summary>
      Method = 'M',
      /// <summary>
      /// Member is a property
      /// </summary>
      Property = 'P',
      /// <summary>
      /// Member is a field
      /// </summary>
      Field = 'F'
    }

    #region Properties

    /// <summary>
    /// Member name
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Member type
    /// </summary>
    MemberType Type { get; }
    /// <summary>
    /// Member documnetation
    /// </summary>
    IDocumentation Documentation { get; } 

    #endregion
  }
}
