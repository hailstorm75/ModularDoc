﻿using System.Collections.Generic;
using ModularDoc.Members.Enums;
using ModularDoc.Members.ResolvedTypes;

namespace ModularDoc.Members.Types
{
  /// <summary>
  /// Interface for types
  /// </summary>
  public interface IType
  {
    /// <summary>
    /// Reflection fullname with namespace
    /// </summary>
    string RawName { get; }

    /// <summary>
    /// Type name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Type namespace
    /// </summary>
    string TypeNamespace { get; }

    /// <summary>
    /// Determines whether this type is nested
    /// </summary>
    bool IsNested { get; }

    /// <summary>
    /// Type accessor
    /// </summary>
    AccessorType Accessor { get; }

    /// <summary>
    /// Determines the dot net type represented by this instance
    /// </summary>
    DotNetType Type { get; }

    /// <summary>
    /// Type attributes
    /// </summary>
    IReadOnlyCollection<IResAttribute> Attributes { get; }
  }
}
