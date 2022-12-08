using System;
using System.Globalization;
using Avalonia.Data.Converters;
using ModularDoc;

namespace ModularDoc.Views.Main.Converters
{
  public class LogTypeToStringConverter
    : IValueConverter
  {
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      if (value is not IModularDocLogger.LogType logType)
        return null!;

      return logType switch
      {
        IModularDocLogger.LogType.Info => "Info",
        IModularDocLogger.LogType.Error => "Error",
        IModularDocLogger.LogType.Warning => "Warning",
        IModularDocLogger.LogType.Debug => "Debug",
        _ => throw new ArgumentOutOfRangeException()
      };
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
      => value switch
      {
        "Info" => IModularDocLogger.LogType.Info,
        "Error" => IModularDocLogger.LogType.Error,
        "Warning" => IModularDocLogger.LogType.Warning,
        "Debug" => IModularDocLogger.LogType.Debug,
        _ => throw new ArgumentOutOfRangeException()
      };
  }
}