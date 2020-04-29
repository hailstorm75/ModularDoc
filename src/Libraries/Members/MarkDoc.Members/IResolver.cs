﻿using System;
using System.Collections.Generic;

namespace MarkDoc.Members
{
  /// <summary>
  /// Type resolver
  /// </summary>
  public interface IResolver
  {
    Lazy<IReadOnlyDictionary<string, IReadOnlyCollection<IType>>> Types { get; }

    /// <summary>
    /// Resolves <paramref name="assembly"/> types
    /// </summary>
    /// <param name="assembly">Path to assembly</param>
    void Resolve(string assembly);

    IResType Resolve(object source);

    IType? FindReference(object source, IResType type);

    IType ResolveType(object subject, object? parent = null);
  }
}
