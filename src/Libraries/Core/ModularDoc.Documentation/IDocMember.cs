﻿namespace ModularDoc.Documentation
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
      Field = 'F',
      /// <summary>
      /// Member is an event
      /// </summary>
      Event ='E'
    }

    #region Properties

    /// <summary>
    /// Member raw name
    /// </summary>
    string RawName { get; }
    /// <summary>
    /// Member display name
    /// </summary>
    string DisplayName { get; }
    /// <summary>
    /// Member type
    /// </summary>
    MemberType Type { get; }
    /// <summary>
    /// Member documentation
    /// </summary>
    IDocumentation Documentation { get; }

    #endregion
  }
}
