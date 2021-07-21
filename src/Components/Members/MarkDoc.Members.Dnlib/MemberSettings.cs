using System;
using System.Collections.Generic;
using MarkDoc.Helpers;

namespace MarkDoc.Members.Dnlib
{
  public struct MemberSettings
    : IMemberSettings
  {
    #region Properties

    /// <inheritdoc />
    public Guid Id { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public string Description { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<string> Paths { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public MemberSettings(IEnumerable<string> paths)
    {
      Id = new Guid();
      Name = "Dnlib type resolver";
      Description = "Reflection based assembly type resolver";
      Paths = paths.ToReadOnlyCollection();
    }
  }
}