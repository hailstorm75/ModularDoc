using System;
using System.Collections.Generic;

namespace ModularDoc.Members.Dnlib
{
  public readonly struct MemberSettings
    : IMemberSettings
  {
    #region Properties

    /// <inheritdoc />
    public Guid Id { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<string> Paths { get; }

    /// <inheritdoc />
    public bool ProcessPrivate { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public MemberSettings(IReadOnlyDictionary<string, string> data)
    {
      Id = new Guid();
      if (!data.TryGetValue(IMemberSettings.ENTRY_PATHS, out var paths))
        // TODO
        throw new ArgumentException();

      // ReSharper disable once PossibleNullReferenceException
      Paths = paths.Split(IMemberSettings.PATH_DELIMITER);

      if (!data.TryGetValue(IMemberSettings.ENTRY_PROCESS_PRIVATE, out var processPrivateString)
          || !bool.TryParse(processPrivateString, out var processPrivate))
        ProcessPrivate = false;
      else
        ProcessPrivate = processPrivate;
    }
  }
}