using System.IO;

namespace MarkDoc.Core
{
  public interface IPlugin
  {
    string Id { get; }

    string Name { get; }

    string Description { get; }

    Stream? Image { get; }
  }
}