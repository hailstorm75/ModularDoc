using MarkDoc.Core;

namespace MarkDoc.Helpers;

public class IndefiniteProcess
  : BaseProcess, IIndefiniteProcess
{
  /// <inheritdoc />
  public IndefiniteProcess(string name)
    : base(name)
  {
  }
}