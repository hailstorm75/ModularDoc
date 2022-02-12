using System;

namespace MarkDoc.Core
{
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
    /// <param name="source">Log source</param>
    void Info(string message, string source = "Unspecified");

    /// <summary>
    /// Logs given <paramref name="message"/> as debug information
    /// </summary>
    /// <param name="message">Message to log</param>
    /// <param name="source">Log source</param>
    void Debug(string message, string source = "Unspecified");

    /// <summary>
    /// Logs given <paramref name="message"/> as an error
    /// </summary>
    /// <param name="message">Message to log</param>
    /// <param name="source">Log source</param>
    void Error(string message, string source = "Unspecified");

    /// <summary>
    /// Logs given <paramref name="message"/> as a warning
    /// </summary>
    /// <param name="message">Message to log</param>
    /// <param name="source">Log source</param>
    void Warning(string message, string source = "Unspecified");
  }
}