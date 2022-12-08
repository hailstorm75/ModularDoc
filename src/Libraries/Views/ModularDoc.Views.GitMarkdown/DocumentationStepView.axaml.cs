using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ModularDoc;
using ModularDoc.Documentation;
using ModularDoc.MVVM.Helpers;

namespace ModularDoc.Views.GitMarkdown
{
  public class DocumentationStepView
    : BaseStepUserControl<IStepViewModel<IDocSettings>, IDocSettings>
  {
  }
}