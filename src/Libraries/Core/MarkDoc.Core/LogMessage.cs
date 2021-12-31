using System;

namespace MarkDoc.Core
{
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
