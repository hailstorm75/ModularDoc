using MarkDoc.Elements.Members.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarkDoc.Elements.Members
{
  public interface IConstructor
    : IMember
  {
    /// <summary>
    /// Method arguments
    /// </summary>
    IReadOnlyCollection<IArgument> Arguments { get; }
  }
}
