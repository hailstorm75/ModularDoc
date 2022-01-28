using System;
using System.Globalization;
using Avalonia.Data.Converters;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Bitmap = Avalonia.Media.Imaging.Bitmap;

namespace MarkDoc.Views.Main.Converters
{
  public class BitmapConverter
    : IValueConverter
  {
    private static Bitmap? ConvertToAvaloniaBitmap(Image? input)
    {
      if (input is null)
        return null;

      var bitmapTmp = new System.Drawing.Bitmap(input);
      var bitMapData = bitmapTmp.LockBits(new Rectangle(0, 0, bitmapTmp.Width, bitmapTmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
      var result = new Bitmap(Avalonia.Platform.PixelFormat.Bgra8888, Avalonia.Platform.AlphaFormat.Premul,
        bitMapData.Scan0,
        new Avalonia.PixelSize(bitMapData.Width, bitMapData.Height),
        new Avalonia.Vector(96, 96),
        bitMapData.Stride);

      bitmapTmp.UnlockBits(bitMapData);
      bitmapTmp.Dispose();

      return result;
    }

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