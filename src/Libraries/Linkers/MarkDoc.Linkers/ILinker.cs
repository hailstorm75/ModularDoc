using System;
using System.Collections.Generic;
using MarkDoc.Members.Members;
using MarkDoc.Members.ResolvedTypes;
using MarkDoc.Members.Types;

namespace MarkDoc.Linkers
{
  /// <summary>
  /// Interface for creating links
  /// </summary>
  public interface ILinker
  {
    /// <summary>
    /// Types path structure
    /// </summary>
    IReadOnlyDictionary<IType, string> Paths { get; }

    #region Methods

    /// <summary>
    /// Creates a link to a given type <paramref name="target"/>
    /// </summary>
    /// <param name="source">Link from</param>
    /// <param name="target">Link target</param>
    /// <returns>Retrieved link</returns>
    string CreateLink(IType source, IResType target);

    /// <summary>
    /// Creates a link to a given type <paramref name="target"/>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <returns>Retrieved link</returns>
    string CreateLink(IType source, IType target);

    /// <summary>
    /// Creates an anchor to a given <paramref name="member"/>
    /// </summary>
    /// <param name="page">Page type</param>
    /// <param name="member">Member to link to</param>
    /// <returns>Retrieved link</returns>
    Lazy<string> CreateAnchor(IType page, IMember member);

    void RegisterAnchor(IMember member, Lazy<string> anchor);

    #endregion
  }
}
