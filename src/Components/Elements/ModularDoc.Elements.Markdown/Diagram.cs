using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ModularDoc.Elements.Markdown
{
  public class Diagram
    : IDiagram
  {
    private readonly string m_name;

    #region Fields

    private readonly string m_content;

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public Diagram(string name, string diagramProvider, string content, bool external, string urlToRaw)
    {
      m_name = name;
      m_content = @$"```{diagramProvider}
  {content}
```";
    }

    /// <inheritdoc />
    public IEnumerable<string> Print()
    {
      yield return m_content;
    }

    /// <inheritdoc />
    public async ValueTask ToExternalFile(string directory)
    {
      if (directory is null)
        throw new ArgumentNullException(nameof(directory));
      if (!Directory.Exists(directory))
        throw new DirectoryNotFoundException(directory);

      var spaceAndType = m_name.Split('.');
      var path = Path.Combine(directory, Path.Combine(spaceAndType.SkipLast(1).ToArray()));
      Directory.CreateDirectory(path);

      await using var file = File.CreateText(Path.Combine(path, spaceAndType.Last() + ".iuml"));
      await file.WriteLineAsync("@startuml").ConfigureAwait(false);
      await file.WriteLineAsync(m_content).ConfigureAwait(false);
      await file.WriteLineAsync("@enduml").ConfigureAwait(false);
    }
  }
}