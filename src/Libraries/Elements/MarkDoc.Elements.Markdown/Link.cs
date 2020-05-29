namespace MarkDoc.Elements.Markdown
{
  public class Link
    : ILink
  {
    #region Properties

    /// <inheritdoc />
    public IText Content { get; }

    /// <inheritdoc />
    public string Reference { get; }

    #endregion

    public Link(IText content, string refernce)
    {
      Content = content;
      Reference = refernce;
    }

    public override string ToString()
      => $"[{Content}]({Reference})";
  }
}
