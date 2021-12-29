using System.Threading;
using MarkDoc.Core;

namespace MarkDoc.Helpers
{
  public class DefiniteProcess
    : BaseProcess, IDefiniteProcess
  {
    private int m_complete;

    /// <inheritdoc />
    public double Complete { get; private set; }

    /// <inheritdoc />
    public int Max { get; }

    /// <inheritdoc />
    public DefiniteProcess(string name, int max) : base(name)
      => Max = max;

    /// <inheritdoc />
    public void IncreaseCompletion()
      => Complete = Interlocked.Increment(ref m_complete) / (double)Max;
  }
}
