﻿namespace ModularDoc.Members.Enums
{
  /// <summary>
  /// Types of member accessors
  /// </summary>
  public enum AccessorType
  {
    /// <summary>
    /// Visible to everyone
    /// </summary>
    Public,
    /// <summary>
    /// Visible to children
    /// </summary>
    Protected,
    /// <summary>
    /// Visible within library
    /// </summary>
    Internal,
    /// <summary>
    /// Visible to children within the library
    /// </summary>
    ProtectedInternal,
    /// <summary>
    /// Visible within the file
    /// </summary>
    File,
    /// <summary>
    /// Private visibility
    /// </summary>
    Private
  }
}
