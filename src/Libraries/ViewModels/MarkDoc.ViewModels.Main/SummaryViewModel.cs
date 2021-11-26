using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using MarkDoc.Constants;
using MarkDoc.MVVM.Helpers;
using ReactiveUI;

namespace MarkDoc.ViewModels.Main
{
  public class SummaryViewModel
    : BaseViewModel, ISummaryViewModel
  {
    private readonly NavigationManager m_navigationManager;
    private IReadOnlyDictionary<string,string> m_pluginSettings = new Dictionary<string, string>();

    #region Properties

    /// <inheritdoc />
    public string Title => "Summary";

    #endregion

    #region Commands

    /// <inheritdoc />
    public ICommand BackCommand { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public SummaryViewModel(NavigationManager navigationManager)
    {
      m_navigationManager = navigationManager;
      BackCommand = ReactiveCommand.Create(NavigateBack);
    }

    /// <inheritdoc />
    public override Task SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {
      m_pluginSettings = arguments;

      return Task.CompletedTask;
    }

    private void NavigateBack()
    {
      switch (m_navigationManager.PreviousPage)
      {
        case PageNames.HOME:
          m_navigationManager.NavigateTo(PageNames.HOME);
          break;
        case PageNames.CONFIGURATION:
          m_navigationManager.NavigateTo(PageNames.HOME, m_pluginSettings);
          break;
      }
    }
  }
}