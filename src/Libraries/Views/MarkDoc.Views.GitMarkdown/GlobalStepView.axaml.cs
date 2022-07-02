using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
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

    public static IEnumerable<TreeViewItem> GetTreeViewItems(TreeView treeView)
    {
      var stackPanel = treeView
        .FindDescendantOfType<TreeViewItem>()
        ?.FindAncestorOfType<StackPanel>();

      var children = stackPanel
        ?.GetVisualChildren()
        .OfType<TreeViewItem>();

      if (children is null)
        yield break;

      foreach (var treeViewItem in children)
      {
        yield return treeViewItem;
        foreach (var item in GetTreeViewItems(treeViewItem))
          yield return item;
      }
    }

    public static IEnumerable<TreeViewItem> GetTreeViewItems(TreeViewItem treeItem)
    {
      yield break;
    }

    private void Button_OnCLick(object? sender, RoutedEventArgs e)
    {
      foreach (var treeViewItem in GetTreeViewItems(m_treeView))
        treeViewItem.IsEnabled = true;
    }
  }
}