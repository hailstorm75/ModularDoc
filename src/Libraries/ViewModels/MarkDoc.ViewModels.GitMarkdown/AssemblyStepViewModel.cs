using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData;
using MarkDoc.Members;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.ViewModels.GitMarkdown
{
  public sealed class AssemblyStepViewModel
    : BaseStepViewModel<IMemberSettings>
  {
    #region Constants

    private const string SETTINGS_PATHS = "paths";
    private const char PATH_DELIM = '|';

    #endregion

    private readonly HashSet<string> m_pathsInsensitive = new (StringComparer.InvariantCultureIgnoreCase);

    #region Properties

    /// <inheritdoc />
    public override string Title => "Assembly processing settings";

    /// <inheritdoc />
    public override string Description => "TODO";

    public ObservableCollection<string> Paths { get; } = new();

    #endregion

    #region Methods

    /// <inheritdoc />
    public override void SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {
      if (!arguments.TryGetValue(SETTINGS_PATHS, out var pathsSerialized))
        return;

      // ReSharper disable once PossibleNullReferenceException
      Paths.AddRange(pathsSerialized.Split(PATH_DELIM));
    }

    private bool AddPath(string path)
    {
      if (m_pathsInsensitive.Contains(path))
        return false;

      m_pathsInsensitive.Add(path);
      Paths.Add(path);

      UpdateCanProceed();

      return true;
    }

    private bool RemovePath(string path)
    {
      if (!m_pathsInsensitive.Contains(path))
        return false;

      m_pathsInsensitive.Remove(path);
      Paths.Remove(path);

      UpdateCanProceed();

      return true;
    }

    private void UpdateCanProceed()
      => OnCanProceed(Paths.Count > 0);

    #endregion
  }
}