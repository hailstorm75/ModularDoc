using MarkDoc.Core;
using ReactiveUI;

namespace MarkDoc.Helpers
{
  public abstract class BaseProcess
    : ReactiveObject, IProcess
  {
    private IProcess.ProcessState m_state;

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public IProcess.ProcessState State
    {
      get => m_state;
      set
      {
        m_state = value;
        this.RaisePropertyChanged(nameof(State));
      }
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    protected BaseProcess(string name)
    {
      Name = name;
    }
  }
}
