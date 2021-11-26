using System.Collections.Generic;

namespace MarkDoc.Core
{
  public interface ISettingsCreator
  {
    T CreateSettings<T>(IReadOnlyDictionary<string, string> data) where T : ILibrarySettings;
  }
}