using System;

namespace MarkDoc.Core
{
  /// <summary>
  /// Interface for settings of libraries
  /// </summary>
  public interface ILibrarySettings
  {
    /// <summary>
    /// Library id
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Library name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Library description
    /// </summary>
    string Description { get; }
  }
}