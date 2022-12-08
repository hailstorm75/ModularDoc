using System;
using System.Globalization;
using Avalonia.Data.Converters;
using System.IO;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace ModularDoc.Views.Main.Converters
{
  public class BitmapConverter
    : IValueConverter
  {
    private static Bitmap? Decode(Stream? stream, string? width)
    => stream is null
      ? default
      : Bitmap.DecodeToWidth(stream, width is not null && int.TryParse(width, out var num) ? num : 256);

    /// <inheritdoc />
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
      => Decode(value as Stream, parameter as string);

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
  }
}