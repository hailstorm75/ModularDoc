using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using ModularDoc;

namespace ModularDoc.Views.Main.Converters
{
  public class ActivePluginStepToBoolConverter
    : AvaloniaObject, IValueConverter
  {
    public static readonly StyledProperty<object?> CurrentPluginProperty
      = AvaloniaProperty.Register<ActivePluginStepToBoolConverter, object?>(nameof(CurrentPlugin));
    public object? CurrentPlugin
    {
      get => GetValue(CurrentPluginProperty);
      set => SetValue(CurrentPluginProperty, value);
    }

    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
      => value is IPluginStep step && CurrentPlugin is IPluginStep currentStep && step.Id.Equals(currentStep.Id);

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
  }
}