using System;
using System.Globalization;
using Avalonia.Data.Converters;
using MarkDoc.Core;

namespace MarkDoc.Views.Main.Converters
{
  public class LogTypeToIconConverter
    : IValueConverter
  {
    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is not IMarkDocLogger.LogType logType)
        return null!;

      return logType switch
      {
        IMarkDocLogger.LogType.Info => "fa-info-circle",
        IMarkDocLogger.LogType.Error => "fa-times-circle ",
        IMarkDocLogger.LogType.Warning => "fa-exclamation-circle",
        IMarkDocLogger.LogType.Debug => "fa-bug",
        _ => throw new ArgumentOutOfRangeException()
      };
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      => value switch
      {
        "fa-info-circle" => IMarkDocLogger.LogType.Info,
        "fa-times-circle " => IMarkDocLogger.LogType.Error,
        "fa-exclamation-circle" => IMarkDocLogger.LogType.Warning,
        "fa-bug" => IMarkDocLogger.LogType.Debug,
        _ => throw new ArgumentOutOfRangeException()
      };
  }
}