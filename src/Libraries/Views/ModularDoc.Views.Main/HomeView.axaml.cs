﻿using Avalonia.Controls;
using ModularDoc.MVVM.Helpers;
using ModularDoc.ViewModels;

namespace ModularDoc.Views.Main
{
  public class HomeView
    : BaseUserControl<IHomeViewModel>, IHomeView
  {
    // ReSharper disable once UnusedParameter.Local
    private void SplitView_OnPaneClosing(object? sender, SplitViewPaneClosingEventArgs  e)
      => e.Cancel = ViewModel.SelectedPlugin is not null;
  }
}