using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.VisualTree;

namespace MarkDoc.MVVM.Helpers
{
  public static class TreeViewHelper
  {
    private static IEnumerable<TreeViewItem> GetTreeViewItems(TreeView treeView)
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

    private static IEnumerable<TreeViewItem> GetTreeViewItems(TreeViewItem treeItem)
    {
      var items = treeItem.FindDescendantOfType<TreeViewItem>()?.GetVisualParent()?.GetVisualChildren()?.OfType<TreeViewItem>();
      if (items is null)
        yield break;

      foreach (var item in items)
      {
        yield return item;
        foreach (var subItem in GetTreeViewItems(item))
          yield return subItem;
      }
    }

    public static void ExpandAllTreeViewItems(this TreeView treeView)
    {
      foreach (var treeViewItem in GetTreeViewItems(treeView))
      {
        treeViewItem.IsExpanded = true;
        treeViewItem.ApplyTemplate();

        var presenter = treeViewItem.FindDescendantOfType<ItemsPresenter>();
        presenter?.ApplyTemplate();
      }
    }

    public static void CollapseAllTreeViewItems(this TreeView treeView)
    {
      foreach (var child in GetTreeViewItems(treeView).Reverse())
        child.IsExpanded = false;
    }
  }
}