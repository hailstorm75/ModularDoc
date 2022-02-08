using System;
using System.Collections;
using System.Globalization;
using Avalonia.Data.Converters;
using Castle.Core.Internal;

namespace MarkDoc.Views.Main.Converters
{
  public class IsEmptyConverter
    : IValueConverter
  {
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      var negate = parameter is string negateString && bool.TryParse(negateString, out var result) && result;

      var evaluation =  value switch
      {
        IEnumerable item => item.IsNullOrEmpty(),
        int number => number == 0,
        _ => false
      };

      return negate
        ? !evaluation
        : evaluation;
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
  }
}