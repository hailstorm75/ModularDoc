namespace MarkDoc.Core
{
  public interface IPluginStep
  {
    string Name { get; }
    int StepNumber { get; }
  }
}