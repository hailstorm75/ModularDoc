using MarkDoc.Elements.Extensions;

namespace MarkDoc.Elements
{
  public interface ILink
    : IElement, IHasContent<IText>
  {
    string Reference { get; set; }
  }
}
