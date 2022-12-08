using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using ModularDoc;

namespace ModularDoc.Views.Main.Converters
{
  public class LogTypeToColorConverter
    : IValueConverter
  {
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      if (value is not IModularDocLogger.LogType logType)
        return null!;

      return logType switch
      {
        IModularDocLogger.LogType.Info => Brushes.MediumBlue,
        IModularDocLogger.LogType.Error => Brushes.Red,
        IModularDocLogger.LogType.Warning => Brushes.GreenYellow,
        IModularDocLogger.LogType.Debug => Brushes.Black,
        _ => throw new ArgumentOutOfRangeException()
      };
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
      => throw new NotSupportedException();
  }
}