using System;

namespace ModularDoc
{
  /// <summary>
  /// Interface for processes
  /// </summary>
  public interface IProcess
  {
    /// <summary>
    /// Invoked whenever the <see cref="State"/> is changed
    /// </summary>
    event EventHandler<ProcessState>? StateChanged;

    /// <summary>
    /// Progress name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// State of the given process
    /// </summary>
    public ProcessState State { get; set; }

    /// <summary>
    /// Enumeration of possible states of a <see cref="IProcess"/>
    /// </summary>
    public enum ProcessState
    {
      /// <summary>
      /// The process is waiting
      /// </summary>
      Idle,
      /// <summary>
      /// The process is running
      /// </summary>
      Running,
      /// <summary>
      /// The process has finished successfully
      /// </summary>
      Success,
      /// <summary>
      /// The process has finished unsuccessfully
      /// </summary>
      Failure,
      /// <summary>
      /// The process has been cancelled
      /// </summary>
      Cancelled
    }
  }
}
