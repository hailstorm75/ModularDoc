using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MarkDoc.Core;
using MarkDoc.Members.Dnlib;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.Views.GitMarkdown
{
  public class AssemblyStepView
    : UserControl, IStepView<IStepViewModel<MemberSettings>, MemberSettings>
  {
    /// <inheritdoc />
    public IStepViewModel<MemberSettings> ViewModel { get; } = TypeResolver.ResolveViewModel<IStepViewModel<MemberSettings>>();

    public AssemblyStepView()
    {
      InitializeComponent();
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }

    /// <inheritdoc />
    public void SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {
      throw new System.NotImplementedException();
    }
  }
}