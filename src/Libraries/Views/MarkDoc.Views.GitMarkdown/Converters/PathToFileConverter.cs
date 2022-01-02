using System;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;

namespace MarkDoc.Views.GitMarkdown.Converters
{
  public class PathToFileConverter
    : IValueConverter
  {
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      if (value is not string path)
        return value!;

      var index = path.LastIndexOf(Path.DirectorySeparatorChar) + 1;
      return index > 0
        ? path[index..]
        : path;
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
  }
}