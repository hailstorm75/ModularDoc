﻿using System;

namespace MarkDoc.Core
{
  /// <summary>
  /// Logger message structure
  /// </summary>
  public readonly struct LogMessage
  {
    #region Properties

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
    /// Log source
    /// </summary>
    public string Source { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public LogMessage(IMarkDocLogger.LogType type, string message, string source)
    {
      Type = type;
      Message = message;
      Source = source;
      Time = DateTime.Now;
    }
  }
}
