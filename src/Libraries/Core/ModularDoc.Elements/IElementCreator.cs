using System;
using System.Collections.Generic;
using static ModularDoc.Elements.IList;

namespace ModularDoc.Elements
{
  /// <summary>
  /// Interface for <see cref="IElement"/> creators
  /// </summary>
  public interface IElementCreator
  {
    /// <summary>
    /// Creates a new <see cref="IList"/> instance
    /// </summary>
    /// <returns>Created instance</returns>
    IList CreateList(IEnumerable<IElement> elements, ListType type, string heading = "", int level = 0);
    /// <summary>
    /// Creates a new <see cref="ISection"/> instance
    /// </summary>
    /// <returns>Created instance</returns>
    ISection CreateSection(IEnumerable<IElement> content, string heading = "", int level = 0);
    /// <summary>
    /// Creates a new <see cref="ITable"/> instance
    /// </summary>
    /// <returns>Created instance</returns>
    ITable CreateTable(IEnumerable<IReadOnlyCollection<IElement>> content, IEnumerable<IText> headings, string heading = "", int level = 0);
    /// <summary>
    /// Creates a new <see cref="IPage"/> instance
    /// </summary>
    /// <returns>Created instance</returns>
    IPage CreatePage(IEnumerable<IPage>? subpages = default, IEnumerable<IElement>? content = default, string heading = "", int level = 0);
    /// <summary>
    /// Creates a new <see cref="IText"/> instance
    /// </summary>
    /// <param name="content">Text content</param>
    /// <param name="style">Text style</param>
    /// <returns>Created instance</returns>
    IText CreateText(string content, IText.TextStyle style = IText.TextStyle.Normal);

    /// <summary>
    /// Creates a new <see cref="IDiagram"/> instance
    /// </summary>
    /// <param name="name">Diagram name</param>
    /// <param name="content">Diagram data source</param>
    /// <returns></returns>
    IDiagram CreateDiagram(string name, string content);

    /// <summary>
    /// Creates a new <see cref="ILink"/> instance
    /// </summary>
    /// <param name="content">Wrapped text</param>
    /// <param name="reference">Link reference</param>
    /// <returns>Created instance</returns>
    ILink CreateLink(IText content, Lazy<string> reference);
    /// <summary>
    /// Joins text <paramref name="content"/> into a single <see cref="ITextContent"/> separated by a <paramref name="delimiter"/>
    /// </summary>
    /// <param name="content">Content to join</param>
    /// <param name="delimiter">Content delimiter</param>
    /// <returns>Joint content</returns>
    ITextContent JoinTextContent(IEnumerable<ITextContent> content, string delimiter);
  }
}
