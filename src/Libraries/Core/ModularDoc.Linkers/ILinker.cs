using System;
using System.Collections.Generic;
using ModularDoc.Members.Members;
using ModularDoc.Members.ResolvedTypes;
using ModularDoc.Members.Types;

namespace ModularDoc.Linkers
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
    /// <param name="source">Link from</param>
    /// <param name="target">Link target</param>
    /// <returns>Retrieved link</returns>
    string CreateLink(IType source, IType target);

    /// <summary>
    /// Creates an anchor to a given <paramref name="member"/>
    /// </summary>
    /// <param name="page">Page type</param>
    /// <param name="member">Member to link to</param>
    /// <returns>Retrieved link</returns>
    Lazy<string> CreateAnchor(IType page, IMember member);

    /// <summary>
    /// Adds an <paramref name="anchor"/> for the given <paramref name="member"/> to known list of anchors
    /// </summary>
    /// <param name="member">Member for which the anchor should be registered</param>
    /// <param name="anchor">Anchor value</param>
    void RegisterAnchor(IMember member, Lazy<string> anchor);

    /// <summary>
    /// Creates a link path to the source code location of the given <paramref name="member"/>
    /// </summary>
    /// <param name="member">Member to link to</param>
    /// <returns>Link to source code line</returns>
    string CreateLinkToSourceCode(IMember member);

    #endregion
  }
}
