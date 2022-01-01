using System;
using System.Globalization;
using Avalonia.Data.Converters;
using MarkDoc.Core;

namespace MarkDoc.Views.Main.Converters
{
  public class ProcessStateToVisibilityConverter
    : IValueConverter
  {
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      if (value is not IProcess.ProcessState state)
        return null!;

      var result = state == IProcess.ProcessState.Running;
      return parameter is null ? result : !result;
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
  }
}