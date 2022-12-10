using ModularDoc;
using ModularDoc.Core;

namespace ModularDoc.MVVM.Helpers
{
  public abstract class BaseStepUserControl<TViewModel, TSettings>
    : BaseUserControl<TViewModel>, IStepView<TViewModel, TSettings>
    where TSettings : ILibrarySettings
    where TViewModel : IStepViewModel<TSettings>
  {
  }
}