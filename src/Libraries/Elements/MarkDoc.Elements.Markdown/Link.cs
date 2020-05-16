namespace MarkDoc.Elements.Markdown
{
  public class Link
    : ILink
  {
    #region Properties

    /// <inheritdoc />
    public IText Content { get; set; }

    /// <inheritdoc />
    public string Reference { get; set; } = string.Empty; 

    #endregion

    public override string ToString()
      => $"[{Content}]({Reference})";
  }
}
