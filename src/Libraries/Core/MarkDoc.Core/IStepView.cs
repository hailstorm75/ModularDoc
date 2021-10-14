namespace MarkDoc.Core
{
  public interface IStepView<out TViewModel, TSettings>
    : IView<TViewModel>
    where TSettings : ILibrarySettings
    where TViewModel : IStepViewModel<TSettings>
  {
  }
}