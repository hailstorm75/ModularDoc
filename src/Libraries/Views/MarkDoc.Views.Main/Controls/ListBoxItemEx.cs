using Avalonia;
using Avalonia.Controls;

namespace MarkDoc.Views.Main.Controls
{
  public class ListBoxItemEx
    : ListBoxItem
  {
    #region Fields

    private object? m_thisItem;
    private object? m_selectedItem;

    #endregion

    #region Direct Properties

    /// <summary>
    /// Defines the <see cref="SelectedItem"/> property.
    /// </summary>
    public static readonly DirectProperty<ListBoxItemEx, object?> SelectedItemProperty =
      AvaloniaProperty.RegisterDirect<ListBoxItemEx, object?>(
        nameof(SelectedItem),
        o => o.SelectedItem,
        (o, v) => o.SelectedItem = v);

    /// <summary>
    /// Defines the <see cref="ThisItem"/> property.
    /// </summary>
    public static readonly DirectProperty<ListBoxItemEx, object?> ThisItemProperty =
      AvaloniaProperty.RegisterDirect<ListBoxItemEx, object?>(
        nameof(ThisItem),
        o => o.ThisItem,
        (o, v) => o.ThisItem = v);

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the selected item.
    /// </summary>
    public object? SelectedItem
    {
      get => m_selectedItem;
      set
      {
        m_selectedItem = value;
        IsSelected = m_selectedItem?.Equals(m_thisItem) ?? false;
      }
    }

    /// <summary>
    /// Gets or sets the selected item.
    /// </summary>
    public object? ThisItem
    {
      get => m_thisItem;
      set
      {
        m_thisItem = value;
        IsSelected = m_selectedItem?.Equals(m_thisItem) ?? false;
      }
    }

    #endregion

    /// <inheritdoc />
    protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
    {
      base.OnPropertyChanged(change);

      switch (change.Property.Name)
      {
        case nameof(ThisItem):
          ThisItem = change.NewValue;
          break;
        case nameof(SelectedItem):
          SelectedItem = change.NewValue;
          break;
      }
    }
  }
}