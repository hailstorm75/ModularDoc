using System;
using ModularDoc;
using ModularDoc.Core;

namespace ModularDoc.Helpers
{
  public class Logger
    : IModularDocLogger
  {
    /// <inheritdoc />
    public event EventHandler<LogMessage>? NewLog;

    /// <inheritdoc />
    public void Info(string message, string source = "Unspecified") => NewLog?.Invoke(this, new LogMessage(IModularDocLogger.LogType.Info ,message, source));

    /// <inheritdoc />
    public void Debug(string message, string source = "Unspecified") => NewLog?.Invoke(this, new LogMessage(IModularDocLogger.LogType.Debug, message, source));

    /// <inheritdoc />
    public void Error(string message, string source = "Unspecified") => NewLog?.Invoke(this, new LogMessage(IModularDocLogger.LogType.Error, message, source));

    /// <inheritdoc />
    public void Warning(string message, string source = "Unspecified") => NewLog?.Invoke(this, new LogMessage(IModularDocLogger.LogType.Warning, message, source));
  }
}