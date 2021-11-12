using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using MarkDoc.Core;

namespace MarkDoc.Views.Main
{
  public class PluginStepToViewConverter
    : IValueConverter
  {
    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is not IPluginStep pluginStep)
        return "ERROR";

      return pluginStep.GetStepView(new Dictionary<string, string>());
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }
}