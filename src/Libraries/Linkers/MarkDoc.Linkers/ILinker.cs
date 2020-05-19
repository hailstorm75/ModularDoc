using MarkDoc.Members;
using System;
using System.Collections.Generic;
using System.Text;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Linkers
{
  public interface ILinker
  {
    string CreateLink(IResType type);
  }
}
