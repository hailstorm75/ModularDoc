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
    public void Info(string message, string source = "Unspecified") => NewLog?.Invoke(this, new LogMessage(IMarkDocLogger.LogType.Info ,message, source));

    /// <inheritdoc />
    public void Debug(string message, string source = "Unspecified") => NewLog?.Invoke(this, new LogMessage(IMarkDocLogger.LogType.Debug, message, source));

    /// <inheritdoc />
    public void Error(string message, string source = "Unspecified") => NewLog?.Invoke(this, new LogMessage(IMarkDocLogger.LogType.Error, message, source));

    /// <inheritdoc />
    public void Warning(string message, string source = "Unspecified") => NewLog?.Invoke(this, new LogMessage(IMarkDocLogger.LogType.Warning, message, source));
  }
}