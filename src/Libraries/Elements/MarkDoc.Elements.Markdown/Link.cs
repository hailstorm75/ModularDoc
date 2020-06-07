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

    public Link(IText content, string reference)
    {
      Content = content;
      Reference = reference;
    }

    public override string ToString()
      => $"[{Content.ToString()}]({Reference})";
  }
}
