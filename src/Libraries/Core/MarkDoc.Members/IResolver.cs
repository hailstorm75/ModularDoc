using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarkDoc.Core;
using MarkDoc.Helpers;
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

    /// <summary>
    /// Resolved namespace trie
    /// </summary>
    Lazy<TrieNamespace> Namespaces { get; }

    #region Methods

    /// <summary>
    /// Resolves all assemblies based on the given settings
    /// </summary>
    /// <returns></returns>
    Task ResolveAsync(IMemberSettings memberSettings, IGlobalSettings globalSettings);

    /// <summary>
    /// Resolves <paramref name="assembly"/> types
    /// </summary>
    /// <param name="assembly">Path to assembly</param>
    void Resolve(string assembly);

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

    /// <summary>
    /// Gets the <paramref name="line"/> number from the <paramref name="source"/> file for the given member <paramref name="token"/>
    /// </summary>
    /// <param name="token">Member token</param>
    /// <param name="line">Member line position in the <paramref name="source"/></param>
    /// <param name="source">Source code file path</param>
    /// <returns>Ture if successfully retrieved requested data</returns>
    bool TryGetMemberSourceLine(int token, out int line, out string source);

    #endregion
  }
}
