using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using ModularDoc;

namespace ModularDoc.Views.Main.Converters
{
  public class ProcessStateToColorConverter
    : IValueConverter
  {
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      if (value is not IProcess.ProcessState state)
        return null!;

      return state switch
      {
        IProcess.ProcessState.Idle => Brushes.Black,
        IProcess.ProcessState.Running => Brushes.Black,
        IProcess.ProcessState.Success => Brushes.Green,
        IProcess.ProcessState.Failure => Brushes.Red,
        IProcess.ProcessState.Cancelled => Brushes.DarkSalmon,
        _ => throw new ArgumentOutOfRangeException()
      };
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
      => throw new NotSupportedException();
  }
}