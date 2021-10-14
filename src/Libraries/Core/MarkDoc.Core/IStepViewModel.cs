namespace MarkDoc.Core
{
  public interface IStepViewModel<in TSettings>
    : IViewModel
    where TSettings : ILibrarySettings
  {
    void SetSettings(TSettings settings);
  }
}