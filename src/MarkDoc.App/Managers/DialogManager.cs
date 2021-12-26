using Avalonia.Controls;
using MarkDoc.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarkDoc.App.Views;
using MarkDoc.Core;
using MarkDoc.MVVM.Helpers;

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
      => await Task.Run(async () =>
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
      }).ConfigureAwait(false);

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

    /// <inheritdoc />
    public async ValueTask<bool> ShowDialogAsync<TView>(IReadOnlyDictionary<string, string>? arguments = default, IDialogManager.DialogButtons buttons = IDialogManager.DialogButtons.OkCancel)
      where TView : IDialogView
    {
      var view = TypeResolver.Resolve<TView>();
      var dialog = new DialogView
      {
        ViewContent = view
      };

      await view.SetNamedArgumentsAsync(arguments ?? new Dictionary<string, string>(0)).ConfigureAwait(false);
      await dialog.ShowDialog(m_windowProvider()).ConfigureAwait(false);

      return dialog.GetDialogResult();
    }

    #endregion
  }
}
