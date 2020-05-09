namespace MarkDoc.Elements.Markdown
{
  public class Link
    : ILink
  {
    #region Properties

    /// <inheritdoc />
    public string Content { get; set; } = string.Empty;

    /// <inheritdoc />
    public string Reference { get; set; } = string.Empty; 

    #endregion

    public override string ToString()
      => $"[{Content}]({Reference})";
  }
}
