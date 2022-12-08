using System.Threading;
using ModularDoc;
using ReactiveUI;

namespace ModularDoc.Helpers
{
  public class DefiniteProcess
    : BaseProcess, IDefiniteProcess
  {
    private int m_current;

    /// <inheritdoc />
    public double Complete { get; private set; }

    /// <inheritdoc />
    public int Current => m_current;

    /// <inheritdoc />
    public int Max { get; }

    /// <inheritdoc />
    public DefiniteProcess(string name, int max) : base(name)
      => Max = max;

    /// <inheritdoc />
    public void IncreaseCompletion()
    {
      Complete = Interlocked.Increment(ref m_current) / (double)Max;

      this.RaisePropertyChanged(nameof(Complete));
      this.RaisePropertyChanged(nameof(Current));
    }
  }
}
