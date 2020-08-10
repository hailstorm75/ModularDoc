using MarkDoc.Elements;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MarkDoc.Generator;
using MarkDoc.Linkers;
using MarkDoc.Members.Types;

namespace MarkDoc.Printer.Markdown
{
  public class PrinterMarkdown
    : IPrinter
  {
    #region Fields

    private const string EXTENSION = ".md";

    private readonly ILinker m_linker;
    private readonly ITypeComposer m_composer;

    #endregion

    public PrinterMarkdown(ITypeComposer composer, ILinker linker)
    {
      m_linker = linker;
      m_composer = composer;
    }

    #region Methods

    /// <inheritdoc />
    public async Task Print(IEnumerable<IType> types, string path)
    {
      Task PrintIntermediate(IType type)
        => Print(m_composer.Compose(type), type, path);

      var tasks = types.AsParallel().Select(PrintIntermediate);

      await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    private async Task Print(IElement element, IType type, string output)
    {
      var path = Path.Combine(output, m_linker.Paths[type]) + EXTENSION;

      if (!Directory.Exists(path))
        Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new DirectoryNotFoundException());

      await using var file = File.CreateText(path);
      foreach (var line in element.Print())
        await file.WriteAsync(line).ConfigureAwait(false);
    }

    #endregion
  }
}
