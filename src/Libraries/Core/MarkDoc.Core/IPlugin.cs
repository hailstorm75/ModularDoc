namespace MarkDoc.Core
{
  public interface IPlugin
  {
    string Name { get; }
    string Description { get; }
    string Image { get; }
  }
}