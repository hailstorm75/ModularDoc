namespace MarkDoc.Core
{
  public interface IStepViewModel<TSettings>
    : IViewModel
    where TSettings : ILibrarySettings
  {
    void SetSettings(TSettings settings);
  }
}