using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;

namespace ModularDoc.Views.Main
{
  internal static class TreeHelper
  {
    public static List<T> GetVisualTreeObjects<T>(this IVisual obj) where T : IVisual
    {
      var objects = new List<T>();
      foreach (var child in obj.GetVisualChildren())
      {
        if (child is T requestedType)
          objects.Add(requestedType);
        objects.AddRange(child.GetVisualTreeObjects<T>());
      }

      return objects;
    }

    public static void ToggleExpander(this DataGrid dataGrid, bool expand)
    {
      var groupHeaderList = GetVisualTreeObjects<DataGridRowGroupHeader>(dataGrid);
      if (groupHeaderList.Count == 0)
        return;

      foreach (var e in groupHeaderList.SelectMany(GetVisualTreeObjects<ToggleButton>))
        e.IsChecked = expand;
    }
  }
}