using MarkDoc.Elements.Members.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarkDoc.Elements.Members
{
  public interface IMember
  {
    /// <summary>
    /// Is method static
    /// </summary>
    bool IsStatic { get; }

    /// <summary>
    /// Member name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Member accessor
    /// </summary>
    AccessorType Accessor { get; }
  }
}
