using MarkDoc.Elements.Extensions;

namespace MarkDoc.Elements
{
  public interface ILink
    : IElement, IHasContent<string>
  {
    string Reference { get; set; }
  }
}
