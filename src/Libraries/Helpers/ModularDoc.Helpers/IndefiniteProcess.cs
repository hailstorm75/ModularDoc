using ModularDoc;
using ModularDoc.Core;

namespace ModularDoc.Helpers
{
  public class IndefiniteProcess
    : BaseProcess, IIndefiniteProcess
  {
    /// <inheritdoc />
    public IndefiniteProcess(string name)
      : base(name)
    {
    }
  }
}