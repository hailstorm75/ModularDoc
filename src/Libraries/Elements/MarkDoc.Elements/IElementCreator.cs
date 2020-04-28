using System;
using System.Collections.Generic;
using System.Text;

namespace MarkDoc.Elements
{
  public interface IElementCreator
  {
    IList CreateList();
    ISection CreateSection();
    ITable CreateTable();
    IPage CreatePage();
    IText CreateText();
  }
}
