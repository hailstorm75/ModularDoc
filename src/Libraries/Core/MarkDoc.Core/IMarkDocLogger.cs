using System;

namespace MarkDoc.Core
{
  public interface IIndefiniteProcess
    : IProcess
  {
  }

  public interface IDefiniteProcess
  {
    
  }

  public interface IProcess
  {
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
      Started,
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

  /// <summary>
  /// Interface for in-house logger
  /// </summary>
  public interface IMarkDocLogger
  {
    /// <summary>
    /// Logging level types
    /// </summary>
    public enum LogType
    {
      /// <summary>
      /// Information logging level
      /// </summary>
      Info,
      /// <summary>
      /// Error logging level
      /// </summary>
      Error,
      /// <summary>
      /// Warning logging level
      /// </summary>
      Warning,
      /// <summary>
      /// Debug logging level
      /// </summary>
      Debug
    }

    /// <summary>
    /// Invoked when a new log is created
    /// </summary>
    event EventHandler<LogMessage> NewLog;

    /// <summary>
    /// Logs given <paramref name="message"/> as information
    /// </summary>
    /// <param name="message">Message to log</param>
    void Info(string message);
    /// <summary>
    /// Logs given <paramref name="message"/> as debug information
    /// </summary>
    /// <param name="message">Message to log</param>
    void Debug(string message);
    /// <summary>
    /// Logs given <paramref name="message"/> as an error
    /// </summary>
    /// <param name="message">Message to log</param>
    void Error(string message);
    /// <summary>
    /// Logs given <paramref name="message"/> as a warning
    /// </summary>
    /// <param name="message">Message to log</param>
    void Warning(string message);
  }

  /// <summary>
  /// Logger message structure
  /// </summary>
  public readonly struct LogMessage
  {
    /// <summary>
    /// Log type
    /// </summary>
    public IMarkDocLogger.LogType Type { get; }

    /// <summary>
    /// Log message
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Log time
    /// </summary>
    public DateTime Time { get; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public LogMessage(IMarkDocLogger.LogType type, string message)
    {
      Type = type;
      Message = message;
      Time = DateTime.Now;
    }
  }
}