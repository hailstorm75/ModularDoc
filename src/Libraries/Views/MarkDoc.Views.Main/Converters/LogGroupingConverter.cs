using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Data.Converters;
using Castle.Core.Internal;
using MarkDoc.Core;

namespace MarkDoc.Views.Main.Converters
{
  public class LogGroupingConverter
    : AvaloniaObject, IValueConverter
  {
    public static readonly StyledProperty<bool> GroupBySourceProperty
      = AvaloniaProperty.Register<ActivePluginStepToBoolConverter, bool>(nameof(GroupBySource));

    public bool GroupBySource
    {
      get => GetValue(GroupBySourceProperty);
      set => SetValue(GroupBySourceProperty, value);
    }

    public static readonly StyledProperty<bool> GroupByTypeProperty
      = AvaloniaProperty.Register<ActivePluginStepToBoolConverter, bool>(nameof(GroupByType));

    public bool GroupByType
    {
      get => GetValue(GroupByTypeProperty);
      set => SetValue(GroupByTypeProperty, value);
    }

    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      if (value is not IReadOnlyCollection<LogMessage> logs || logs.IsNullOrEmpty())
        return value;

      var grouping = new DataGridCollectionView(logs);
      if (GroupBySource)
        grouping.GroupDescriptions.Add(new DataGridPathGroupDescription(nameof(LogMessage.Source)));
      if (GroupByType)
        grouping.GroupDescriptions.Add(new DataGridPathGroupDescription(nameof(LogMessage.Type)));

      return grouping;
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
  }
}
