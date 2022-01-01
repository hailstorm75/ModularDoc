using System;
using System.Globalization;
using Avalonia.Data.Converters;
using MarkDoc.Core;

namespace MarkDoc.Views.Main.Converters
{
  public class ProcessStateToStringConverter
    : IValueConverter
  {
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      if (value is not IProcess.ProcessState state)
        return null!;

      return state switch
      {
        IProcess.ProcessState.Idle => "Idle",
        IProcess.ProcessState.Running => "Running",
        IProcess.ProcessState.Success => "Success",
        IProcess.ProcessState.Failure => "Failure",
        IProcess.ProcessState.Cancelled => "Cancelled",
        _ => throw new ArgumentOutOfRangeException()
      };
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
      if (value is not string state)
        return null!;

      return state switch
      {
        "Idle" => IProcess.ProcessState.Idle,
        "Running" => IProcess.ProcessState.Running,
        "Success" => IProcess.ProcessState.Success,
        "Failure" => IProcess.ProcessState.Failure,
        "Cancelled" => IProcess.ProcessState.Cancelled,
        _ => throw new ArgumentOutOfRangeException()
      };
    }
  }
}