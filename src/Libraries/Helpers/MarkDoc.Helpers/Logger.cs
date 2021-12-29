using System;
using MarkDoc.Core;

namespace MarkDoc.Helpers
{
  public class Logger
    : IMarkDocLogger
  {
    /// <inheritdoc />
    public event EventHandler<LogMessage>? NewLog;

    /// <inheritdoc />
    public void Info(string message) => NewLog?.Invoke(this, new LogMessage(IMarkDocLogger.LogType.Info ,message));

    /// <inheritdoc />
    public void Debug(string message) => NewLog?.Invoke(this, new LogMessage(IMarkDocLogger.LogType.Debug, message));

    /// <inheritdoc />
    public void Error(string message) => NewLog?.Invoke(this, new LogMessage(IMarkDocLogger.LogType.Error, message));

    /// <inheritdoc />
    public void Warning(string message) => NewLog?.Invoke(this, new LogMessage(IMarkDocLogger.LogType.Warning, message));
  }
}