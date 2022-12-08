using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ModularDoc.Views.GitMarkdown.Converters
{
  public class NegateConverter
    : IValueConverter
  {
    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      if (value is not bool boolean)
        return value;

      return !boolean;
    }

    /// <inheritdoc />
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      if (value is not bool boolean)
        return value;

      return !boolean;
    }
  }
}