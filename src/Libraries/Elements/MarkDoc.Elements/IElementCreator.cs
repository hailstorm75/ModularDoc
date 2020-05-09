namespace MarkDoc.Elements
{
  public interface IElementCreator
  {
    IList CreateList();
    ISection CreateSection();
    ITable CreateTable();
    IPage CreatePage();
    IText CreateText();
    ILink CreateLink();
  }
}
