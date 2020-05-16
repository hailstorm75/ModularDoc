namespace MarkDoc.Elements
{
  public interface IElementCreator
  {
    IList CreateList();
    ISection CreateSection();
    ITable CreateTable();
    IPage CreatePage();
    IText CreateText(string content, IText.TextStyle style = IText.TextStyle.Normal);
    ILink CreateLink(IText content, string reference = "");
  }
}
