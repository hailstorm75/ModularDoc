using System;
using System.Globalization;
using Avalonia.Data.Converters;
using MarkDoc.Core;

namespace MarkDoc.Views.Main.Converters
{
  public class LogTypeToStringConverter
    : IValueConverter
  {
    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is not IMarkDocLogger.LogType logType)
        return null!;

      return logType switch
      {
        IMarkDocLogger.LogType.Info => "Info",
        IMarkDocLogger.LogType.Error => "Error",
        IMarkDocLogger.LogType.Warning => "Warning",
        IMarkDocLogger.LogType.Debug => "Debug",
        _ => throw new ArgumentOutOfRangeException()
      };
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      => value switch
      {
        "Info" => IMarkDocLogger.LogType.Info,
        "Error" => IMarkDocLogger.LogType.Error,
        "Warning" => IMarkDocLogger.LogType.Warning,
        "Debug" => IMarkDocLogger.LogType.Debug,
        _ => throw new ArgumentOutOfRangeException()
      };
  }
}