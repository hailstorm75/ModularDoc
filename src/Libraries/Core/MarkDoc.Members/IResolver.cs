using System;
using System.Collections.Generic;
using MarkDoc.Members.ResolvedTypes;
using MarkDoc.Members.Types;

namespace MarkDoc.Members
{
  /// <summary>
  /// Type resolver
  /// </summary>
  public interface IResolver
  {
    /// <summary>
    /// Resolved types
    /// </summary>
    Lazy<IReadOnlyDictionary<string, IReadOnlyCollection<IType>>> Types { get; }

    #region Methods

    /// <summary>
    /// Resolves <paramref name="assembly"/> types
    /// </summary>
    /// <param name="assembly">Path to assembly</param>
    void Resolve(string assembly);

    /// <summary>
    /// Resolves type to a <see cref="IResType"/>
    /// </summary>
    /// <param name="source">Type to resolve</param>
    /// <param name="generics">Dictionary of type generics</param>
    /// <returns>Resolved type</returns>
    IResType Resolve(object source, IReadOnlyDictionary<string, string>? generics = null);

    /// <summary>
    /// Links a <paramref name="type"/> instance to a <see name="IType"/> instance
    /// </summary>
    /// <param name="source">Source of <paramref name="type"/></param>
    /// <param name="type">Type to link to</param>
    /// <remarks>
    /// This method can be called after of the <see cref="Types"/> have been resolved.
    /// Calling during resolution of <see cref="Types"/> will render incorrect results.
    /// <para/>
    /// Utilize lazy loading to overcome this issue
    /// </remarks>
    /// <returns>Linked <see name="IType"/> instance. Null if unresolved.</returns>
    IType? FindReference(object source, IResType type);

    /// <summary>
    /// Resolves given <paramref name="subject"/> to a type
    /// </summary>
    /// <param name="subject">Subject to resolve</param>
    /// <param name="parent">Parent of <paramref name="subject"/></param>
    /// <returns>Resolved type</returns>
    IType ResolveType(object subject, object? parent = null);

    /// <summary>
    /// Attempts to find a type using its <paramref name="fullname"/>
    /// </summary>
    /// <param name="fullname">Full type name</param>
    /// <param name="result">Found type</param>
    /// <remarks>
    /// The <paramref name="fullname"/> should be equal to <see cref="IType.RawName"/>
    /// </remarks>
    /// <returns>True if found</returns>
    bool TryFindType(string fullname, out IType? result);

    #endregion
  }
}
