﻿using MarkDoc.Elements.Members.Enums;

namespace MarkDoc.Elements.Members
{
  /// <summary>
  /// Interface for type members
  /// </summary>
  public interface IMember
  {
    /// <summary>
    /// Is member obsolete
    /// </summary>
    bool IsObsolete { get; }

    /// <summary>
    /// Is method static
    /// </summary>
    bool IsStatic { get; }

    /// <summary>
    /// Member name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Member accessor
    /// </summary>
    AccessorType Accessor { get; }
  }
}
