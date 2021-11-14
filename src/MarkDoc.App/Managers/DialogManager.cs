using Avalonia.Controls;
using MarkDoc.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarkDoc.App.Managers
{
  /// <summary>
  /// Manager for UI dialogs
  /// </summary>
  public class DialogManager
    : IDialogManager
  {
    private readonly Func<Window> m_windowProvider;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="window">Window root</param>
    public DialogManager(Func<Window> window) => m_windowProvider = window;

    #region Method

    /// <inheritdoc />
    public async ValueTask<Option<IReadOnlyCollection<string>>> TrySelectFilesAsync(
      string title,
      IEnumerable<(IEnumerable<string> extensions, string description)> filters,
      bool multiselect = false)
    {
      var dialog = new OpenFileDialog
      {
        Title = title,
        AllowMultiple = multiselect,
      };

      foreach (var (extensions, description) in filters)
      {
        var dialogFilter = new FileDialogFilter
        {
          Name = description,
        };
        dialogFilter.Extensions.AddRange(extensions);

        dialog.Filters.Add(dialogFilter);
      }

      var result = await dialog.ShowAsync(m_windowProvider());
      return result is null
        ? Option<IReadOnlyCollection<string>>.OfEmpty()
        : Option<IReadOnlyCollection<string>>.Of(result);
    }

    /// <inheritdoc />
    public async ValueTask<Option<string>> TrySelectFolderAsync(string title)
    {
      var dialog = new OpenFolderDialog
      {
        Title = title
      };

      var result = await dialog.ShowAsync(m_windowProvider());
      return result is null
        ? Option<string>.OfEmpty()
        : Option<string>.Of(result);
    }

    #endregion
  }
}
