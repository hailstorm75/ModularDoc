using ModularDoc;

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