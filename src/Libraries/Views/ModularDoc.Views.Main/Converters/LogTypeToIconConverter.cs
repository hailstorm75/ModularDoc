using System;
using System.Globalization;
using Avalonia.Data.Converters;
using ModularDoc;

namespace ModularDoc.Views.Main.Converters
{
  public class LogTypeToIconConverter
    : IValueConverter
  {
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      if (value is not IModularDocLogger.LogType logType)
        return null!;

      return logType switch
      {
        IModularDocLogger.LogType.Info => "fa-info-circle",
        IModularDocLogger.LogType.Error => "fa-times-circle",
        IModularDocLogger.LogType.Warning => "fa-exclamation-circle",
        IModularDocLogger.LogType.Debug => "fa-bug",
        _ => throw new ArgumentOutOfRangeException()
      };
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
      => value switch
      {
        "fa-info-circle" => IModularDocLogger.LogType.Info,
        "fa-times-circle " => IModularDocLogger.LogType.Error,
        "fa-exclamation-circle" => IModularDocLogger.LogType.Warning,
        "fa-bug" => IModularDocLogger.LogType.Debug,
        _ => throw new ArgumentOutOfRangeException()
      };
  }
}