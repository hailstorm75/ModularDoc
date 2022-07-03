using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MarkDoc.Core;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.Views.GitMarkdown
{
  public class GlobalStepView
    : BaseStepUserControl<IStepViewModel<IGlobalSettings>, IGlobalSettings>
  {
    private readonly TreeView m_treeView;

    /// <summary>
    /// Default constructor
    /// </summary>
    public GlobalStepView()
    {
      AvaloniaXamlLoader.Load(this);
      m_treeView = this.FindControl<TreeView>("AvailableNamespacesTree");
    }

    private void ButtonExpandAll_OnCLick(object? sender, RoutedEventArgs e)
      => m_treeView.ExpandAllTreeViewItems();

    private void ButtonCollapseAll_OnCLick(object? sender, RoutedEventArgs e)
      => m_treeView.CollapseAllTreeViewItems();
  }
}