using System;
using System.Collections.Generic;
using MarkDoc.Helpers;

namespace MarkDoc.Members.Dnlib
{
  public readonly struct MemberSettings
    : IMemberSettings
  {
    #region Properties

    /// <inheritdoc />
    public Guid Id { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<string> Paths { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public MemberSettings(IEnumerable<string> paths)
    {
      Id = new Guid();
      Paths = paths.ToReadOnlyCollection();
    }
  }
}