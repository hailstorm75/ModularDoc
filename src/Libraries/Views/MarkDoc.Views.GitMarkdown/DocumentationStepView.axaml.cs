using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MarkDoc.Core;
using MarkDoc.Documentation;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.Views.GitMarkdown
{
  public class DocumentationStepView
    : BaseStepUserControl<IStepViewModel<IDocSettings>, IDocSettings>
  {
  }
}