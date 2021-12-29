using System;
using System.Globalization;
using Avalonia.Data.Converters;
using MarkDoc.Core;

namespace MarkDoc.Views.Main.Converters
{
  public class ProcessStateToIconConverter
    : IValueConverter
  {
    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is not IProcess.ProcessState state)
        return null!;

      return state switch
      {
        IProcess.ProcessState.Idle => "fa-clock-o",
        IProcess.ProcessState.Started => "fa-spinner",
        IProcess.ProcessState.Success => "fa-check-circle-o",
        IProcess.ProcessState.Failure => "fa-times-circle-o",
        IProcess.ProcessState.Cancelled => "fa-ban",
        _ => throw new ArgumentOutOfRangeException()
      };
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }
}