using System;
using ModularDoc;
using ModularDoc.Core;
using ReactiveUI;

namespace ModularDoc.Helpers
{
  public abstract class BaseProcess
    : ReactiveObject, IProcess
  {
    private IProcess.ProcessState m_state;

    /// <inheritdoc />
    public event EventHandler<IProcess.ProcessState>? StateChanged;

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public IProcess.ProcessState State
    {
      get => m_state;
      set
      {
        m_state = value;
        OnStateChanged(value);
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

    private void OnStateChanged(IProcess.ProcessState e)
      => StateChanged?.Invoke(this, e);
  }
}
