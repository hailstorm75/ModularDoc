using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using MarkDoc.Core;

namespace MarkDoc.Views.Main.Converters
{
  public class LogTypeToColorConverter
    : IValueConverter
  {
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      if (value is not IMarkDocLogger.LogType logType)
        return null!;

      return logType switch
      {
        IMarkDocLogger.LogType.Info => Brushes.MediumBlue,
        IMarkDocLogger.LogType.Error => Brushes.Red,
        IMarkDocLogger.LogType.Warning => Brushes.GreenYellow,
        IMarkDocLogger.LogType.Debug => Brushes.Black,
        _ => throw new ArgumentOutOfRangeException()
      };
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
      => throw new NotSupportedException();
  }
}