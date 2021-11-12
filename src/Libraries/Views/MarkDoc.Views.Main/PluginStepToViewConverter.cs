using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Data.Converters;
using MarkDoc.Core;

namespace MarkDoc.Views.Main
{
  public class PluginStepToViewConverter
    : AvaloniaObject, IValueConverter
  {
    public static readonly DirectProperty<PluginStepToViewConverter, IEnumerable> PluginSettingsProperty =
      AvaloniaProperty.RegisterDirect<PluginStepToViewConverter, IEnumerable>(
        nameof(PluginSettings),
        o => o.PluginSettings,
        (o, v) => o.PluginSettings = v);
    private IEnumerable m_pluginSettings = new AvaloniaList<object>();
    public IEnumerable PluginSettings
    {
      get => m_pluginSettings;
      set => SetAndRaise(PluginSettingsProperty, ref m_pluginSettings, value);
    }

    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is not IPluginStep pluginStep
          || m_pluginSettings is not IReadOnlyDictionary<string, Dictionary<string, string>> pluginSettings
          || !pluginSettings.TryGetValue(pluginStep.Id, out var settings))
        return "ERROR";

      // ReSharper disable once AssignNullToNotNullAttribute
      return pluginStep.GetStepView(settings);
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }
}